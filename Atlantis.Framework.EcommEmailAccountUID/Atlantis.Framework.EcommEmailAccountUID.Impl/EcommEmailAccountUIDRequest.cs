using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommEmailAccountUID.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommEmailAccountUID.Impl
{
  public class EcommEmailAccountUIDRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_getEmailExternalResourceIDByOrderID_sp";
    private const string _SHOPPERIDPARAM = "order_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        List<EmailResults> resultValues = new List<EmailResults>();
        EcommEmailAccountUIDRequestData request = (EcommEmailAccountUIDRequestData)oRequestData;
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.OrderID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();            
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                int resourceID = Convert.ToInt32(reader[0]);
                int emailPFid = Convert.ToInt32(reader[1]);
                string externalResourceID = Convert.ToString(reader[2]);
                resultValues.Add(new EmailResults(resourceID, emailPFid, externalResourceID));
              }
            }
          }
        }
        oResponseData = new EcommEmailAccountUIDRsponseData(resultValues);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommEmailAccountUIDRsponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommEmailAccountUIDRsponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
