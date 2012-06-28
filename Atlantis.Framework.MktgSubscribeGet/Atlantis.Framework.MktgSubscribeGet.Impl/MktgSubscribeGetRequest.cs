using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSubscribeGet.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MktgSubscribeGet.Impl
{
  public class MktgSubscribeGetRequest : IRequest
  {
    private const string GET_STORED_PROD = "gdshop_mktg_contactGetPubListByHashAndPrivateLabel_sp";

    private static string HashEmail(string email)
    {
      if (email == string.Empty)
      {
        return string.Empty;
      }
      var managed = new SHA256Managed();
      var encoding = new ASCIIEncoding();

      byte[] buffer = managed.ComputeHash(encoding.GetBytes(email));
      byte[] bytes = new byte[0x40];

      const int num = 0x20;
      int num3 = num * 2;
      for (int i = num - 1; i >= 0; i--)
      {
        byte num4 = buffer[i];
        bytes[--num3] = (byte)((num4 % 10) + 0x61);
        bytes[--num3] = (byte)((num4 / 10) + 0x61);
      }
      return encoding.GetString(bytes);
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;

      MktgSubscribeGetRequestData mktgRequest = (MktgSubscribeGetRequestData)requestData;
      IDictionary<int, MktgSubscribeOptIn> optInDictionary = new Dictionary<int, MktgSubscribeOptIn>(64);

      try
      {
        if (string.IsNullOrEmpty(mktgRequest.Email))
        {
          throw new Exception("Email cannot be null or empty.");
        }

        string connectionString = NetConnect.LookupConnectInfo(config);

        using(SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(GET_STORED_PROD, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int) mktgRequest.RequestTimeout.TotalSeconds;

            SqlParameter emailHashParameter = new SqlParameter("contactHash", SqlDbType.VarChar);
            emailHashParameter.Value = HashEmail(mktgRequest.Email);

            SqlParameter privateLabelId = new SqlParameter("PrivateLabelId", SqlDbType.Int);
            privateLabelId.Value = mktgRequest.PrivateLabelId;

            command.Parameters.Add(emailHashParameter);
            command.Parameters.Add(privateLabelId);

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
              DataTable optInDataTable = new DataTable();
              adapter.Fill(optInDataTable);

              foreach (DataRow optInRow in optInDataTable.Rows)
              {
                int id = optInRow["gdshop_mktg_publication_id"] is DBNull ? -1 : Convert.ToInt32(optInRow["gdshop_mktg_publication_id"]);
                string name = optInRow["publication_name"] is DBNull ? string.Empty : Convert.ToString(optInRow["publication_name"]);

                if(id > 0)
                {
                  optInDictionary[id] = new MktgSubscribeOptIn(id, name);
                }
              }
            }
          }
        }

        result = new MktgSubscribeGetResponseData(optInDictionary);
      }
      catch (Exception ex)
      {
        result = new MktgSubscribeGetResponseData(requestData, ex);
      }

      return result;
    }
  }
}