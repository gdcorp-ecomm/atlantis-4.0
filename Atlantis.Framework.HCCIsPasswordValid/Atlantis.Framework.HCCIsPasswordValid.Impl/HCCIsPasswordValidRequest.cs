using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCIsPasswordValid.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCIsPasswordValid.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCIsPasswordValid.Impl
{
  public class HCCIsPasswordValidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCIsPasswordValidRequestData apiRequestData = requestData as HCCIsPasswordValidRequestData;

      try
      {
        HCCAPIService ws = new HCCAPIWebService.HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.HostingAccountSetupResponse apiResponse = ws.IsPasswordValid(apiRequestData.Password);

        if (apiResponse != null)
        {
          responseData = new HCCIsPasswordValidResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(apiRequestData,
            "HCCIsPasswordValidRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCIsPasswordValidResponseData(apiRequestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCIsPasswordValidResponseData(apiRequestData, ex);
      }

      return responseData;
    }

    HCCIsPasswordValidResponse GetHCCResponse(HCCAPIWebService.HostingAccountSetupResponse apiResponse)
    {
      HCCIsPasswordValidResponse response = new HCCIsPasswordValidResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.Errors = apiResponse.Errors;
      return response;
    }
  }
}
