using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public static class HttpRequestEngineTrace
  {
    public static void Register()
    {
      HttpProviderContainer.Instance.RegisterProvider<IEngineTraceProvider, HttpRequestEngineTraceProvider>();
      Engine.OnRequestCompleted += Engine_OnRequestCompleted;
    }

    static void Engine_OnRequestCompleted(ICompletedRequest completedRequest)
    {
      if ((HttpContext.Current != null) && (completedRequest != null))
      {
        try
        {
          IEngineTraceProvider trace;
          if (HttpProviderContainer.Instance.TryResolve(out trace))
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
