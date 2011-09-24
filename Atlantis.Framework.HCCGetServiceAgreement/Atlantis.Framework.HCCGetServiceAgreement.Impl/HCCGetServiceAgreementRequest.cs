using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCGetServiceAgreement.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCGetServiceAgreement.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCGetServiceAgreement.Impl
{
  public class HCCGetServiceAgreementRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCGetServiceAgreementRequestData serviceRequestData = requestData as HCCGetServiceAgreementRequestData;
      try
      {

        HCCAPIService ws = new HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)serviceRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.AccountSetupLicenseResponse apiResponse = ws.GetServiceAgreement(serviceRequestData.AccountUid);

        if (apiResponse != null)
        {
          responseData = new HCCGetServiceAgreementResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(serviceRequestData,
            "HCCGetServiceAgreementRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCGetServiceAgreementResponseData(serviceRequestData, ex);
        }
       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCGetServiceAgreementResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCGetServiceAgreementResponseData(requestData, ex);
      }
       
      return responseData;
    }

   HCCGetServiceAgreementResponse GetHCCResponse(HCCAPIWebService.AccountSetupLicenseResponse apiResponse)
   {
     HCCGetServiceAgreementResponse response = new HCCGetServiceAgreementResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
     response.Agreement = apiResponse.ServiceAgreementText;
     return response;
   }

  }
}
