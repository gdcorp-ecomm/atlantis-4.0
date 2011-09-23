using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HCCAvailDomains.Interface;
using Atlantis.Framework.HCCAvailDomains.Impl.HCCAPIWebService;
using Atlantis.Framework.HCC.Interface;

namespace Atlantis.Framework.HCCAvailDomains.Impl
{
  public class HCCAvailDomainsRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCAvailDomainsRequestData serviceRequestData = requestData as HCCAvailDomainsRequestData;
      try
      {

        HCCAPIService ws = new HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)serviceRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.AvailableDomainsResponse apiResponse = ws.GetAvailableDomains(serviceRequestData.AccountUid, serviceRequestData.PageSize, serviceRequestData.Page);

        if (apiResponse != null)
        {
          responseData = new HCCAvailDomainsResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(serviceRequestData,
            "HCCGetServiceAgreementRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCAvailDomainsResponseData(serviceRequestData, ex);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCAvailDomainsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCAvailDomainsResponseData(requestData, ex);
      }

      return responseData;
    }

   HCCAvailDomainsResponse GetHCCResponse(HCCAPIWebService.AvailableDomainsResponse apiResponse)
   {
     HCCAvailDomainsResponse response = new HCCAvailDomainsResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
     response.Domains = apiResponse.Domains;
     response.Page = apiResponse.Page;
     response.PageSize = apiResponse.PageSize;
     response.TotalDomains = apiResponse.TotalDomains;
   
     return response;
   }
  }
}
