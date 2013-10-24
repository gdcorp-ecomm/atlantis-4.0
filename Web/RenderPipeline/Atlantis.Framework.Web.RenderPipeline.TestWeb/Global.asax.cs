﻿using System;
using System.Web;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline.TestWeb
{
  public class Global : HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      HttpProviderContainer.Instance.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
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