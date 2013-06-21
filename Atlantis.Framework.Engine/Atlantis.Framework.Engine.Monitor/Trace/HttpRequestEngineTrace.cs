using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web;
using System.Diagnostics;

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
        catch
        {
          // Logging here could cause a risk of an infinite loop
        }
      }
    }
  }
}
