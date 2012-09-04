using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.ExpressCheckoutProfileGet.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ExpressCheckoutProfileGet.Impl
{
  public class ExpressCheckoutProfileGetRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperInstantPurchaseProfileGet_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;      
      int profileId = -1;
      try
      {
        ExpressCheckoutProfileGetRequestData request = (ExpressCheckoutProfileGetRequestData)oRequestData;
        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("s_shopper_id", request.ShopperID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
              if(!int.TryParse(reader["pp_instantPurchaseShopperProfileID"].ToString(), out profileId))
              {
                profileId = -1;
              }
            }
            reader.Close();          
          }
        }

        oResponseData = new ExpressCheckoutProfileGetResponseData(profileId);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ExpressCheckoutProfileGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ExpressCheckoutProfileGetResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private string LookupConnectionString(ExpressCheckoutProfileGetRequestData request, ConfigElement config)
    {
      string result = string.Empty;
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");
      if (!String.IsNullOrEmpty(dataSource) && !String.IsNullOrEmpty(applicationName) &&
        !String.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "ExpressCheckoutProfileGetRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
      }

      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        throw new AtlantisException(request, "LookupConnectionString",
                "Database connection string lookup failed", "No ConnectionFound For:"
                + dataSource + ":"
                + applicationName
                + ":" + certificateName);
      }

      return result;
    }
  }
 }
