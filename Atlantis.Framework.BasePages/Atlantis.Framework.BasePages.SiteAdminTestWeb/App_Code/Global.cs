using System;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.BasePages.SiteAdmin.Providers;

public partial class Global : System.Web.HttpApplication
{
  public Global()
  {
  }

  void Application_Start(object sender, EventArgs e)
  {
    HttpProviderContainer.Instance.RegisterProvider<ISiteContext, SiteAdminContextProvider>();
    HttpProviderContainer.Instance.RegisterProvider<IShopperContext, SiteAdminUserProvider>();
    HttpProviderContainer.Instance.RegisterProvider<IDebugContext, DebugProvider>();
    HttpProviderContainer.Instance.RegisterProvider<IManagerContext, NoManagerProvider>();
  }

  void Application_End(object sender, EventArgs e)
  {
  }

  void Application_Error(object sender, EventArgs e)
  {
  }


  void Session_Start(object sender, EventArgs e)
  {
  }

  void Session_End(object sender, EventArgs e)
  {
  }

}
