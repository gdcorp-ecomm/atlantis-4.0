using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCSet404ErrorBehavior.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCSet404ErrorBehavior.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSet404ErrorBehavior.Impl
{
  public class HCCSet404ErrorBehaviorRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCSet404ErrorBehaviorRequestData apiRequestData = requestData as HCCSet404ErrorBehaviorRequestData;
      try
      {
        HCCAPIService ws = new HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HostingResponse apiResponse = ws.Set404ErrorBehavior(apiRequestData.AccountUid,
                                                        (ErrorBehaviorPageType)apiRequestData.ErrorPageType,
                                                        apiRequestData.ErrorPagePath,
                                                        apiRequestData.Filename);

        if (apiResponse != null)
        {
          responseData = new HCCSet404ErrorBehaviorResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(apiRequestData,
            "HCCSet404ErrorBehaviorRequest.RequestHandler",
            "API Response is null or 404 Error Settings are null",
            string.Empty);

          responseData = new HCCSet404ErrorBehaviorResponseData(apiRequestData, ex);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCSet404ErrorBehaviorResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new HCCSet404ErrorBehaviorResponseData(requestData, ex);
      }
      return responseData;
    }

    #endregion

    HCCSet404ErrorBehaviorResponse GetHCCResponse(HostingResponse apiResponse)
    {
      HCCSet404ErrorBehaviorResponse response = new HCCSet404ErrorBehaviorResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);

      return response;
    }
  }
}
