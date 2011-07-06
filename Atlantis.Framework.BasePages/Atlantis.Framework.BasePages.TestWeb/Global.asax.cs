using System;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.BasePages.Providers;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, PrivateLabelAwareSiteContextProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, ShopperContextProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IDebugContext, DebugProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, GdgManagerContextProvider>();
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