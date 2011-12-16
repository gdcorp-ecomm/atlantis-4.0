using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAction.Impl.MyaActionService;
using Atlantis.Framework.MyaAction.Interface;

namespace Atlantis.Framework.MyaAction.Impl
{
  public class MyaActionRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaActionResponseData responseData = null;
      try
      {
        var request = (MyaActionRequestData)requestData;
        using (var service = new WSCmyaActionService())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string result1 = service.QueueAction(String.Concat(request.ActionArgs, "|", request.ActionXml));
          string result2 = service.QueueActionEvent(request.ActionArgs);
          if (!result1.Contains("SUCCESS")|| !result2.Contains("SUCCESS"))
          {
            responseData = new MyaActionResponseData(new AtlantisException(requestData, "MyaAction.RequestHandler", "Error invoking MyaAction", string.Format("QueueAction response: {0}; QueueActionEvent response {1}", result1, result2)));
          }
          else
          {
            responseData = new MyaActionResponseData();
          }
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MyaActionResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MyaActionResponseData(requestData, ex);
      }

      return responseData;
    }

  }
}