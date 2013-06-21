using System;
using Atlantis.Framework.Engine.Monitor.Trace;
using Atlantis.Framework.Engine.Monitor.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public class Global : System.Web.HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, SiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IDebugContext, DebugProvider>();
      HttpRequestEngineTrace.Register();

      System.Web.Routing.RouteTable.Routes.MapEngineMonitorHandler();
    }

    protected void Session_Start(object sender, EventArgs e)
    {

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_PostAcquireRequestState(object sender, EventArgs e)
    {
    }

    protected void Application_Error(object sender, EventArgs e)
    {

    }

    protected void Session_End(object sender, EventArgs e)
    {

    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
  }
}