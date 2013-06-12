using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public static class HttpRequestEngineTrace
  {
    public static void Initialize()
    {
      HttpProviderContainer.Instance.RegisterProvider<EngineTraceProvider, EngineTraceProvider>();
      Engine.OnRequestCompleted += Engine_OnRequestCompleted;
    }

    static void Engine_OnRequestCompleted(ICompletedRequest completedRequest)
    {
      if ((HttpContext.Current != null) && (completedRequest != null))
      {
        try
        {
          EngineTraceProvider trace;
          if (HttpProviderContainer.Instance.TryResolve<EngineTraceProvider>(out trace))
          {
            trace.LogCompletedRequest(completedRequest);
          }
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("HttpRequestEngineTrace.OnRequestCompleted", 0, ex.Message + ex.StackTrace, string.Empty);
        }
      }
    }
  }
}
