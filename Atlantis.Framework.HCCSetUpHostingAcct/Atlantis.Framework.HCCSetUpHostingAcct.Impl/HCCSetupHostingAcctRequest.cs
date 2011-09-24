using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCSetUpHostingAcct.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCSetUpHostingAcct.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetUpHostingAcct.Impl
{
  public class HCCSetupHostingAcctRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var setupAccountRequest = requestData as HCCSetUpHostingAcctRequestData;

      try
      {
        if (setupAccountRequest != null)
        {
          var iisVersion = (IISVersion) setupAccountRequest.IISVersion;
          var dotNetVersion = (ASPDotNetVersion) setupAccountRequest.DotNetVersion;
          var phpVersion = (PHPVersion)  setupAccountRequest.PHPVersion;
          var pipelineMode = (PipeLineMode) setupAccountRequest.PipeLineMode;

          var ws = new HCCAPIWebService.HCCAPIService
                     {
                       Url = ((WsConfigElement) config).WSURL,
                       Timeout = (int) setupAccountRequest.RequestTimeout.TotalMilliseconds
                     };
          HCCAPIWebService.HostingAccountSetupResponse apiResponse = ws.SetUpHostingAccount(setupAccountRequest.AccountUid,
                                                                                            setupAccountRequest.UserName,
                                                                                            setupAccountRequest.Password,
                                                                                            setupAccountRequest.Domain,
                                                                                            iisVersion,
                                                                                            dotNetVersion,
                                                                                            phpVersion,
                                                                                            pipelineMode,
                                                                                            setupAccountRequest.SSLCertificateUid,
                                                                                            setupAccountRequest.EnableGoogleWMT,
                                                                                            setupAccountRequest.EnablePreviewDNS);
        
          if (apiResponse != null)
          {
            responseData = new HCCSetUpHostingAcctResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            var ex = new AtlantisException(setupAccountRequest,
                                                         "HCCSetupHostingAcctRequest.RequestHandler",
                                                         "API Response is null or AccountList is null",
                                                         string.Empty);

            responseData = new HCCSetUpHostingAcctResponseData(setupAccountRequest, ex);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCSetUpHostingAcctResponseData(setupAccountRequest, ex);
      }

      return responseData;
    }

    static HCCSetUpHostingAccountResponse GetHCCResponse(HCCAPIWebService.HostingAccountSetupResponse apiResponse)
    {
      var response = new HCCSetUpHostingAccountResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode)
                       {Errors = apiResponse.Errors ?? new string[0]};
      return response;
    }
  }
}
