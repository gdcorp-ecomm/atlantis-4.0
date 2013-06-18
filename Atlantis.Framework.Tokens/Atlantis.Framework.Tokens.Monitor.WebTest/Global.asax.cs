using System;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Tokens.Monitor.Web;

namespace Atlantis.Framework.Tokens.Monitor.WebTest
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, SiteContext>();

      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      System.Web.Routing.RouteTable.Routes.MapTokensMonitorHandler();
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