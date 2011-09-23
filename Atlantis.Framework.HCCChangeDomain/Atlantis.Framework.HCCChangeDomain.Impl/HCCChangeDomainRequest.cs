using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCChangeDomain.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCChangeDomain.Impl
{
  public class HCCChangeDomainRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCChangeDomainRequestData apiRequestData = requestData as HCCChangeDomainRequestData;
      try
      {
        HCCAPIWebService.HCCAPIService HCCWebService = new HCCAPIWebService.HCCAPIService();
        HCCWebService.Url = ((WsConfigElement)config).WSURL;
        HCCWebService.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;

        HCCAPIWebService.DomainManagementResponse apiResponse = HCCWebService.ChangeDomainName(apiRequestData.AccountUid, 
          apiRequestData.DomainUid, apiRequestData.NewDomainName, apiRequestData.PreviewDns, apiRequestData.NewDomainPath);

        if (apiResponse != null)
        {
          responseData = new HCCChangeDomainResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          string errorDescription = "API Response is null";
          string data = string.Format("apiRequestData.AccountUid {0}, apiRequestData.DomainUid {1}, apiRequestData.NewDomainName {2}," + 
            " apiRequestData.PreviewDns {3}, apiRequestData.NewDomainPath {4}"
            , apiRequestData.AccountUid, apiRequestData.DomainUid, apiRequestData.NewDomainName, apiRequestData.PreviewDns, apiRequestData.NewDomainPath);

          AtlantisException ex = new AtlantisException(requestData,
            "HCCChangeDomainRequest.RequestHandler",
            errorDescription,
            data);

          responseData = new HCCChangeDomainResponseData(requestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCChangeDomainResponseData(requestData, ex);
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
