using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public static class EngineTrace
  {
    public static void Initialize()
    {
      HttpProviderContainer.Instance.RegisterProvider<EngineTraceProvider, EngineTraceProvider>();
      Engine.OnProcessRequestStart += Engine_OnProcessRequestStart;
      Engine.OnProcessRequestComplete += Engine_OnProcessRequestComplete;
    }

    private static void Engine_OnProcessRequestStart(RequestData requestData, int requestType, Guid requestId)
    {
      var traceProvider = HttpProviderContainer.Instance.Resolve<EngineTraceProvider>();
      traceProvider.Engine_OnProcessRequestStart(requestData, requestType, requestId);
    }

    private static void Engine_OnProcessRequestComplete(RequestData requestData, int requestType, Guid requestId, IResponseData responseData)
    {
      var traceProvider = HttpProviderContainer.Instance.Resolve<EngineTraceProvider>();
      traceProvider.Engine_OnProcessRequestComplete(requestData, requestType, requestId, responseData);
    }
  }
}
