using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.ExpressCheckoutDelete.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ExpressCheckoutDelete.Impl
{
  public class ExpressCheckoutDeleteRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperInstantPurchaseProfileDelete_sp";
    private const string _SHOPPERIDPARAM = "s_shopper_id";    

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        ExpressCheckoutDeleteRequestData request = (ExpressCheckoutDeleteRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));            
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            command.ExecuteNonQuery();
          }
        }

        oResponseData = new ExpressCheckoutDeleteResponseData(true);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ExpressCheckoutDeleteResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ExpressCheckoutDeleteResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private string LookupConnectionString(ExpressCheckoutDeleteRequestData request, ConfigElement config)
    {
      string result = string.Empty;

      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");
      if (!String.IsNullOrEmpty(dataSource) && !String.IsNullOrEmpty(applicationName) &&
        !String.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "ExpressCheckoutDeleteRequest.LookupConnectionString",
                                           ConnectLookupType.NetConnectionString);
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
