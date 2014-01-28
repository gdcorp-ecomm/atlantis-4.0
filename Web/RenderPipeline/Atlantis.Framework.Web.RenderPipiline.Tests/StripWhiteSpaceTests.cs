using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Web.RenderPipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Web.RenderPipiline.Tests
{
  [TestClass]
  public class StripWhiteSpaceTests
  {
    const string AFTERCONTENT = "<div></div>\n<div></div>";
    const string BEFORECONTENT = @"      

<div></div>
      
      
      
      
      
      <div></div>    

";

    readonly MockProviderContainer _container = new MockProviderContainer();

    private void InitializeProvidersContexts()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IShopperContext, MockShopperContext>();
      _container.RegisterProvider<IManagerContext, MockManagerContext>();
      _container.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();
      _container.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();

      IAppSettingsProvider settings = _container.Resolve<IAppSettingsProvider>();
    }

    private void SetUrl(string url)
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void StripsWhiteSpace_AppSettingOn()
    {
      InitializeProvidersContexts();
      SetUrl("http://www.godaddy.com");
      _container.SetData<bool>("MockSiteContextSettings.IsRequestInternal", false);
      _container.SetData<string>("MockAppSettingsProvider.ReturnValue", "true");
      IRenderPipelineProvider renderPipelineProvider = _container.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(BEFORECONTENT, new List<IRenderHandler> { new StripWhiteSpaceRenderHandler() });
      Assert.AreEqual(renderedContent, AFTERCONTENT);
    }

    [TestMethod]
    public void StripsWhiteSpace_AppSettingOff()
    {
      InitializeProvidersContexts();
      SetUrl("http://www.godaddy.com");
      _container.SetData<bool>("MockSiteContextSettings.IsRequestInternal", false);
      _container.SetData<string>("MockAppSettingsProvider.ReturnValue", "false");
      IRenderPipelineProvider renderPipelineProvider = _container.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(BEFORECONTENT, new List<IRenderHandler> { new StripWhiteSpaceRenderHandler() });
      Assert.AreEqual(renderedContent, BEFORECONTENT);
    }

    [TestMethod]
    public void StripsWhiteSpace_AppSettingQASpoofOn()
    {
      InitializeProvidersContexts();
      SetUrl("http://www.godaddy.com?QA--RenderPipeline_StripWhiteSpace=true");
      _container.SetData<bool>("MockSiteContextSettings.IsRequestInternal", true);
      _container.SetData<string>("MockAppSettingsProvider.ReturnValue", "false");
      IRenderPipelineProvider renderPipelineProvider = _container.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(BEFORECONTENT, new List<IRenderHandler> { new StripWhiteSpaceRenderHandler() });
      Assert.AreEqual(renderedContent, AFTERCONTENT);
    }

    [TestMethod]
    public void StripsWhiteSpace_AppSettingQASpoofOff()
    {
      InitializeProvidersContexts();
      SetUrl("http://www.godaddy.com?QA--RenderPipeline_StripWhiteSpace=false");
      _container.SetData<bool>("MockSiteContextSettings.IsRequestInternal", true);
      _container.SetData<string>("MockAppSettingsProvider.ReturnValue", "true");
      IRenderPipelineProvider renderPipelineProvider = _container.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(BEFORECONTENT, new List<IRenderHandler> { new StripWhiteSpaceRenderHandler() });
      Assert.AreEqual(renderedContent, BEFORECONTENT);
    }
  }
}
