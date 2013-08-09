using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.DomainGetResourceDomainId.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.DomainGetResourceDomainId.Impl
{
  public class DomainGetResourceDomainIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DomainGetResourceDomainIdResponseData responseData;
      try
      {
        var domainRequestData = (DomainGetResourceDomainIdRequestData)requestData;
        ValidateRequest(domainRequestData);

        string domainId = string.Empty;
        string billingResourceId = string.Empty;

        string connectionString = NetConnect.LookupConnectInfo(config);
        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand("[godaddyBilling].[dbo].[gdshop_billingDomainIDGetByShopperDomainOrderID_sp]", connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@domain", domainRequestData.Domain);
            command.Parameters.AddWithValue("@order_id", domainRequestData.DomainOrderId);
            command.Parameters.AddWithValue("@shopper_id", domainRequestData.DomainOwnerShopperId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
              var oDomain = reader["domainId"];
              var oBillingResource = reader["resource_id"];
              domainId = oDomain != DBNull.Value ? oDomain.ToString() : string.Empty;
              billingResourceId = oBillingResource != DBNull.Value ? oBillingResource.ToString() : string.Empty;
            }

            reader.Dispose();
          }
        }

        responseData = new DomainGetResourceDomainIdResponseData(domainId, billingResourceId);
      }
      catch (AtlantisException aex)
      {
        responseData = new DomainGetResourceDomainIdResponseData(aex);
      }
      catch (Exception ex)
      {
        responseData = new DomainGetResourceDomainIdResponseData(ex, requestData);
      }

      return responseData;
    }

    private void ValidateRequest(DomainGetResourceDomainIdRequestData requestData)
    {
      if (string.IsNullOrEmpty(requestData.Domain) || string.IsNullOrEmpty(requestData.DomainOrderId) || string.IsNullOrEmpty(requestData.DomainOwnerShopperId))
      {
        string errorMessage = string.Concat("Domain:", requestData.Domain, "|DomainOrderId:", requestData.DomainOrderId,
                                            "|DomainOwnerShopperId:", requestData.DomainOwnerShopperId);
        throw new AtlantisException(requestData, "DomainGetResourceDomainIdRequest::ValidateRequest", "Missing data in request", errorMessage);
      }
    }

  }


}

