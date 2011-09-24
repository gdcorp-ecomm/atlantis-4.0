using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCGet404ErrorBehavior.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCGet404ErrorBehavior.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCGet404ErrorBehavior.Impl
{
  public class HCCGet404ErrorBehaviorRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCGet404ErrorBehaviorRequestData serviceRequestData = requestData as HCCGet404ErrorBehaviorRequestData;

      try
      {
        HCCAPIService ws = new HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)serviceRequestData.RequestTimeout.TotalMilliseconds;
        ErrorPageResponse apiResponse = ws.Get404ErrorBehavior(serviceRequestData.AccountUid);

        if (apiResponse != null)
        {
          responseData = new HCCGet404ErrorBehaviorResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(serviceRequestData,
            "HCCGet404ErrorBehaviorRequest.RequestHandler",
            "API Response is null or 404 Error Settings are null",
            string.Empty);

          responseData = new HCCGet404ErrorBehaviorResponseData(serviceRequestData, ex);
        }

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCGet404ErrorBehaviorResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new HCCGet404ErrorBehaviorResponseData(requestData, ex);
      }

      return responseData;
    }

    #endregion

    HCCGet404ErrorBehaviorResponse GetHCCResponse(ErrorPageResponse apiResponse)
    {
      HCCGet404ErrorBehaviorResponse response = new HCCGet404ErrorBehaviorResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.PageType = apiResponse.PageType;
      response.Path = apiResponse.Path;
      response.FileName = apiResponse.FileName;
      response.ErrorNumber = apiResponse.ErrorNumber;

      return response;
    }
  }
}
