using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCIsUsernameValid.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCIsUsernameValid.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCIsUsernameValid.Impl
{
  public class HCCIsUsernameValidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCIsUsernameValidRequestData apiRequestData = requestData as HCCIsUsernameValidRequestData;

      try
      {
        HCCAPIService ws = new HCCAPIWebService.HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.HostingAccountSetupResponse apiResponse = ws.IsUsernameValid(apiRequestData.UserName);

        if (apiResponse != null)
        {
          responseData = new HCCIsUsernameValidResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(apiRequestData,
            "HCCIsUsernameValidRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCIsUsernameValidResponseData(apiRequestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCIsUsernameValidResponseData(apiRequestData, ex);
      }

      return responseData;
    }

    HCCIsUsernameValidResponse GetHCCResponse(HCCAPIWebService.HostingAccountSetupResponse apiResponse)
    {
      HCCIsUsernameValidResponse response = new HCCIsUsernameValidResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.Errors = apiResponse.Errors;
      return response;
    }
  }
}
