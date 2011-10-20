using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.SmartSpaceRenewalBasketXml.Interface;

namespace Atlantis.Framework.SmartSpaceRenewalBasketXml.Impl
{
  public class SmartSpaceRenewalBasketXmlRequest : IRequest
  {
    private const string PROC_NAME = "gdshop_smartDomainGetByDomainResourceID_sp";
    private const string DOMAIN_BILLING_RESOURCE_ID_PARAM = "@domain_resource_id";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      SmartSpaceRenewalBasketXmlResponseData responseData;
      try
      {
        int smartSpaceResourceId = GetSmartDomainResourceId((SmartSpaceRenewalBasketXmlRequestData) oRequestData, oConfig);
        responseData = new SmartSpaceRenewalBasketXmlResponseData(smartSpaceResourceId, oRequestData);
      }
      catch(AtlantisException atlEx)
      {
        responseData = new SmartSpaceRenewalBasketXmlResponseData(atlEx, oRequestData);
      }
      catch(Exception ex)
      {
        responseData = new SmartSpaceRenewalBasketXmlResponseData(ex, oRequestData);
      }

      return responseData;
    }

    private int GetSmartDomainResourceId(SmartSpaceRenewalBasketXmlRequestData requestData, ConfigElement configElement)
    {
      int smartSpaceResourceId = 0;

      if (requestData.ResourceIdType == SmartSpaceRenewalBasketXmlRequestData.BillingResourceIdType.SmartSpace)
      {
        // No need to do a database lookup since we already have the smart space billing resource id
        smartSpaceResourceId = requestData.BillingResourceId;
      }
      else if (requestData.ResourceIdType == SmartSpaceRenewalBasketXmlRequestData.BillingResourceIdType.Domain)
      {
        string connectionString = LookupConnectionString(configElement);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int) requestData.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter(DOMAIN_BILLING_RESOURCE_ID_PARAM, requestData.BillingResourceId));
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader != null && reader.HasRows)
              {
                while (reader.Read())
                {
                  object smartSpaceResourceIdObject = reader[0];

                  if (smartSpaceResourceIdObject == null)
                  {
                    throw new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                "Error retrieving smartSpaceResourceId for domainBillingResourceId: " +
                                                requestData.BillingResourceId,
                                                string.Empty);
                  }

                  smartSpaceResourceId = Convert.ToInt32(smartSpaceResourceIdObject);
                }
              }
            }
          }
        }
      }

      return smartSpaceResourceId;
    }

    private static string LookupConnectionString(ConfigElement config)
    {
      string result = NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"), config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "DomainBundleIdRequest.LookupConnectionString",
                                            ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }

      return result;
    }
  }
}