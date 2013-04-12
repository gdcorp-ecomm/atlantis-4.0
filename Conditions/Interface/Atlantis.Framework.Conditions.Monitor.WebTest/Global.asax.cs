using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Conditions.Monitor.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;

namespace Atlantis.Framework.Conditions.Monitor.WebTest
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, SiteContext>();

      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
      System.Web.Routing.RouteTable.Routes.MapConditionsMonitorHandler();
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