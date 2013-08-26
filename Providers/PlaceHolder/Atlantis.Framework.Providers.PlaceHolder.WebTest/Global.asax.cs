﻿using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Testing.MockProviders;

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