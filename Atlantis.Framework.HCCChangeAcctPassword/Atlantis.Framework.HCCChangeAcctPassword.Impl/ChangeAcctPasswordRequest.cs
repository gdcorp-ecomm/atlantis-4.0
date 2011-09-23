using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCChangeAcctPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCChangeAcctPassword.Impl
{
  public class ChangeAcctPasswordRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCChangeAcctPasswordRequestData apiRequestData = requestData as HCCChangeAcctPasswordRequestData;
      try
      {
        HCCAPIWebService.HCCAPIService HCCWebService = new HCCAPIWebService.HCCAPIService();
        HCCWebService.Url = ((WsConfigElement)config).WSURL;
        HCCWebService.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.ChangeHostingPasswordResponse apiResponse = HCCWebService.ChangeHostingAccountPassword(apiRequestData.AccountUid, apiRequestData.Password);

        if (apiResponse != null)
        {
          responseData = new HCCChangeAcctPasswordResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          string errorDescription = "API Response is null";
          string data = string.Format("AccountUid: {0}, RequestedPassword: {1}", apiRequestData.AccountUid, apiRequestData.Password);

          //if (apiResponse != null)
          //{
          //  errorDescription = string.Format("Error: {0}, Status: {1}, StatusCode: {2}",
          //    apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
          //}

          AtlantisException ex = new AtlantisException(requestData,
            "HCCGetAccountListRequest.RequestHandler",
            errorDescription,
            data);

          responseData = new HCCChangeAcctPasswordResponseData(requestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCChangeAcctPasswordResponseData(requestData, ex);
      }

      return responseData;
    }

    HCCAcctPasswordResponse GetHCCResponse(HCCAPIWebService.ChangeHostingPasswordResponse apiResponse)
    {
      HCCAcctPasswordResponse response = new HCCAcctPasswordResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.Errors = apiResponse.Errors ?? new string[0];

      return response;
    }
  }
}
