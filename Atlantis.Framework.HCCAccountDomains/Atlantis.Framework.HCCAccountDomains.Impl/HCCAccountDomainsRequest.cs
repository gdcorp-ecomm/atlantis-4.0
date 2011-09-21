using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCAccountDomains.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAccountDomains.Impl
{
  public class HCCAccountDomainsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCAccountDomainsRequestData apiRequestData = requestData as HCCAccountDomainsRequestData;
      try
      {
        HCCAPIWebService.HCCAPIService HCCWebService = new HCCAPIWebService.HCCAPIService();
        HCCWebService.Url = ((WsConfigElement)config).WSURL;
        HCCWebService.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.DomainManagementResponse apiResponse = HCCWebService.GetAccountDomains(apiRequestData.AccountUid);

        if (apiResponse != null)
        {
          responseData = new HCCAccountDomainsResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          string errorDescription = "API Response is null";
          string data = string.Format("AccountUid: {0}", apiRequestData.AccountUid);

          AtlantisException ex = new AtlantisException(requestData,
            "HCCGetAccountListRequest.RequestHandler",
            errorDescription,
            data);

          responseData = new HCCAccountDomainsResponseData(requestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCAccountDomainsResponseData(requestData, ex);
      }

      return responseData;
    }

    HCCDomainMgmtResponse GetHCCResponse(HCCAPIWebService.DomainManagementResponse apiResponse)
    {
      HCCDomainMgmtResponse response = new HCCDomainMgmtResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.AccountDomainsXml = apiResponse.AccountDomains;
      response.DomainNameChangeAllowed = apiResponse.DomainNameChangeAllowed;
      response.ModifiedDomains = apiResponse.ModifiedDomains ?? new string[0];
      response.Warnings = apiResponse.Warnings ?? new string[0];

      return response;
    }
  }
}
