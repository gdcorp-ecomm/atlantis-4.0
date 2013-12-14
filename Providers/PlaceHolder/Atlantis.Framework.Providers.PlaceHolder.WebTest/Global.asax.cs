using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public class Global : HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ICDSContentProvider, CDSContentProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IPersonalizationProvider, PersonalizationProvider>();

      HttpProviderContainer.Instance.SetData<bool>("MockSiteContextSettings.IsRequestInternal", true);
      
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