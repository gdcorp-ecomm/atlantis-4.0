using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PrivacyRenewalBasketXml.Impl.EComPrivacyRenewalXmlService;
using Atlantis.Framework.PrivacyRenewalBasketXml.Interface;

namespace Atlantis.Framework.PrivacyRenewalBasketXml.Impl
{
  public class PrivacyRenewalBasketXmlRequest : IRequest
  {
    private const string PROC_NAME = "gdshop_billingGetBundleID_sp";
    private const string DOMAIN_BILLING_RESOURCE_ID_PARAM = "@domain_resource_id";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      string responseXml = string.Empty;
      string errorXml = string.Empty;
      PrivacyRenewalBasketXmlResponseData responseData;
      wscgdDomainProtectionXMLService oEComWeb = null;

      try
      {
        PrivacyRenewalBasketXmlRequestData oRequest = (PrivacyRenewalBasketXmlRequestData)oRequestData;
        oEComWeb = new wscgdDomainProtectionXMLService();
        oEComWeb.Url = ((WsConfigElement)oConfig).WSURL;
        oEComWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = oEComWeb.GetBundleXML(oRequest.ToXML(), out errorXml);

        // We have to make a call to the GoDaddyBilling database to get the protection parent_bundle_id
        if(oRequest.RenewalType == PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Protection)
        {
          int parentBundleId = LookUpProtectionBundleId(oRequest,oConfig);
          responseData = new PrivacyRenewalBasketXmlResponseData(responseXml, parentBundleId, oRequestData, errorXml);
        }
        else
        {
          responseData = new PrivacyRenewalBasketXmlResponseData(responseXml, oRequestData, errorXml);
        }
      }
      catch (Exception ex)
      {
        responseData = new PrivacyRenewalBasketXmlResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (oEComWeb != null)
        {
          oEComWeb.Dispose();
        }
      }

      return responseData;
    }

    private int LookUpProtectionBundleId(PrivacyRenewalBasketXmlRequestData request, ConfigElement oConfig)
    {
      int parentBundleId = 0;

      string connectionString = LookupConnectionString(request, oConfig);

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = 5;
          command.Parameters.Add(new SqlParameter(DOMAIN_BILLING_RESOURCE_ID_PARAM, request.DomainBillingResourceId));
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              object parentBundleIdObject = reader[0];

              if (parentBundleIdObject == null)
              {
                throw new AtlantisException(request,
                                            MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                            "Error retrieving parentBundleId for domainBillingResourceId: " + request.DomainBillingResourceId,
                                            string.Empty);
              }

              parentBundleId = Convert.ToInt32(parentBundleIdObject);
            }
          }
        }
      }

      return parentBundleId;
    }

    private static string LookupConnectionString(PrivacyRenewalBasketXmlRequestData request, ConfigElement oConfig)
    {
      string nimitzConnectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(nimitzConnectionString))
      {
        throw new AtlantisException(request, "LookupConnectionString",
                                    "Database connection string lookup failed", 
                                    string.Empty);
      }
      return nimitzConnectionString;
    }
  }
}
