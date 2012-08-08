using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PLVatID.Interface;

namespace Atlantis.Framework.PLVatID.Impl
{
  public class PLVatIDRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getPlVatID_sp";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PLVatIDResponseData response = null;
      try
      {
        var request = (PLVatIDRequestData)requestData;
        string vatId = string.Empty;

        string connectionString = NetConnect.LookupConnectInfo(config);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@privateLabelID", request.PrivateLabelId));

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              if (reader.HasRows)
              {
                int DESCRIPTION = reader.GetOrdinal("VatID");

                while (reader.Read())
                {
                  if (!reader.IsDBNull(DESCRIPTION))
                 {
                   vatId = reader.GetString(DESCRIPTION);
                 }
                }
              }
            }
          }
        }

        response = new PLVatIDResponseData(vatId);

      }
      catch (AtlantisException aex)
      {
        response = new PLVatIDResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new PLVatIDResponseData(requestData, ex);
      }

      return response;
    }

  }

}
