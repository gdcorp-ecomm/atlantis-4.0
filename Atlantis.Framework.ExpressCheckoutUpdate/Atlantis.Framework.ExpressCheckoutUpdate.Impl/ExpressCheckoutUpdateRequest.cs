using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.ExpressCheckoutUpdate.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ExpressCheckoutUpdate.Impl
{
  public class ExpressCheckoutUpdateRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperInstantPurchaseProfileAddUpdate_sp";
    private const string _SHOPPERIDPARAM = "s_shopper_id";
    private const string _PROFILEIDPARAM = "n_pp_instantPurchaseShopperProfileID";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;      

      try
      {
        ExpressCheckoutUpdateRequestData request = (ExpressCheckoutUpdateRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_PROFILEIDPARAM, request.PaymentProfileId));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            command.ExecuteNonQuery();                       
          }
        }

        oResponseData = new ExpressCheckoutUpdateResponseData(true);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ExpressCheckoutUpdateResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ExpressCheckoutUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private string LookupConnectionString(ExpressCheckoutUpdateRequestData request, ConfigElement config)
    {
      string result = string.Empty;
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");
      if (!String.IsNullOrEmpty(dataSource) && !String.IsNullOrEmpty(applicationName) &&
        !String.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "ExpressCheckoutUpdateRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
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
