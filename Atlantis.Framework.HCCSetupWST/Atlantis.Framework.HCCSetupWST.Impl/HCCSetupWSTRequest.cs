using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCSetupWST.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCSetupWST.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetupWST.Impl
{
  public class HCCSetupWSTRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var setupAccountRequest = requestData as HCCSetupWSTRequestData;

      try
      {
        if (setupAccountRequest != null)
        {
          var ws = new HCCAPIService
          {
            Url = ((WsConfigElement)config).WSURL,
            Timeout = (int)setupAccountRequest.RequestTimeout.TotalMilliseconds
          };
          var apiResponse = ws.SetUpWebsiteTonightAccount(setupAccountRequest.AccountUid,
                                                          setupAccountRequest.UserName,
                                                          setupAccountRequest.Password,
                                                          setupAccountRequest.Domain,
                                                          setupAccountRequest.SslCertificateUid,
                                                          setupAccountRequest.EnableGoogleWmt);

          if (apiResponse != null)
          {
            responseData = new HCCSetupWSTResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            var ex = new AtlantisException(setupAccountRequest,
                                                         "HCCSetupWSTRequest.RequestHandler",
                                                         "API Response is null or AccountList is null",
                                                         string.Empty);

            responseData = new HCCSetupWSTResponseData(setupAccountRequest, ex);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCSetupWSTResponseData(setupAccountRequest, ex);
      }

      return responseData;
    }

    static HCCSetUpHostingAccountResponse GetHCCResponse(HostingAccountSetupResponse apiResponse)
    {
      var response = new HCCSetUpHostingAccountResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode)
                       {Errors = apiResponse.Errors ?? new string[0]};
      return response;
    }
  }
}
