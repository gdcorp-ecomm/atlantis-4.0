using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommInstantPurchaseGet.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EcommInstantPurchaseGet.Impl
{
  public class EcommInstantPurchaseGetRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperInstantPurchaseProfileGet_sp";
    private const string _SHOPPERIDPARAM = "s_shopper_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      int instantPurchaseProfileId = 0;

      try
      {
        EcommInstantPurchaseGetRequestData request = (EcommInstantPurchaseGetRequestData)oRequestData;

        string connectionString = LookupConnectionString(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                object instantPurchaseProfileIdObj = reader[0];
                instantPurchaseProfileId = Convert.ToInt32(instantPurchaseProfileIdObj);
                break;
              }
            }
          }
        }

        oResponseData = new EcommInstantPurchaseGetResponseData(instantPurchaseProfileId);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommInstantPurchaseGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommInstantPurchaseGetResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private static string LookupConnectionString(ConfigElement config)
    {
      string result = NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"), config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "ResourceInfoByProfileRequest.LookupConnectionString",
                                           ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(result) || result.Length <= 1)
      {
        throw new Exception("Invalid Connection String");
      }

      return result;
    }
  }
}
