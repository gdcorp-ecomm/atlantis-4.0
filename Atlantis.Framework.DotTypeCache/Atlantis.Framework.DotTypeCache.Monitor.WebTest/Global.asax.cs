using System;
using Atlantis.Framework.DotTypeCache.Monitor.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;

namespace Atlantis.Framework.DotTypeCache.Monitor.WebTest
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, SiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, ShopperContext>();
      System.Web.Routing.RouteTable.Routes.MapDotTypeCacheMonitorHandler();
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