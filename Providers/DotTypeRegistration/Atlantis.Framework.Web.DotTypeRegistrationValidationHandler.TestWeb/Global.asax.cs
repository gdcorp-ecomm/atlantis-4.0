using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Atlantis.Framework.BasePages.Providers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.DotTypeRegistration;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Web.DynamicRouteHandler;

namespace Atlantis.Framework.Web.DotTypeRegistrationValidationHandler.TestWeb
{
  public class Global : HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      //HttpProviderContainer.Instance.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeRegistrationProvider, DotTypeRegistrationProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, PrivateLabelAwareSiteContextProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, Providers.Shopper.ShopperContextProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, GdgManagerContextProvider>();

      DynamicRouteHandlerRegistrationManager.AutoRegisterRouteHandlers(RouteTable.Routes, new List<string> { "Atlantis.Framework.Providers.DotTypeRegistrationValidationHandler*" }, null);
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