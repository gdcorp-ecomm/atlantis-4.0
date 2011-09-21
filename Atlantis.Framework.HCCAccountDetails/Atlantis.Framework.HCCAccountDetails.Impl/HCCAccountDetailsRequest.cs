using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCAccountDetails.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCAccountDetails.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAccountDetails.Impl
{
  public class HCCAccountDetailsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCAccountDetailsRequestData serviceRequestData = requestData as HCCAccountDetailsRequestData;
      try
      {

        HCCAPIService ws = new HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)serviceRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.HostingAccountDetailsResponse apiResponse = ws.GetHostingAccountDetails(serviceRequestData.AccountUid);

        if (apiResponse != null)
        {
          responseData = new HCCAccountDetailsResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(serviceRequestData,
            "HCCGetServiceAgreementRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCAccountDetailsResponseData(serviceRequestData, ex);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCAccountDetailsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCAccountDetailsResponseData(requestData, ex);
      }

      return responseData;
    }

    HCCAccountDetailsResponse GetHCCResponse(HCCAPIWebService.HostingAccountDetailsResponse apiResponse)
    {
      HCCAccountDetailsResponse response = new HCCAccountDetailsResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.BandwidthAllotment = apiResponse.BandwidthAllotment;
      response.DiskspaceAllotment = apiResponse.DiskspaceAllotment;
      response.Domain = apiResponse.Domain;
      response.IsFreeHosting = apiResponse.FreeHosting;
      response.MSSqlDatabasesAvailable = apiResponse.MSSQLDatabasesAvailable;
      response.MySqlDatabasesAvailable = apiResponse.MySQLDatabasesAvailable;
      response.OperatingSystem = apiResponse.OperatingSystem;
      response.ProductName = apiResponse.ProductName;

      return response;
    }
  }
}
