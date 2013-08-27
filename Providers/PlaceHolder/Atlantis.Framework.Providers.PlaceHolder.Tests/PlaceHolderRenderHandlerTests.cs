using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Tests.RenderContent;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class PlaceHolderRenderHandlerTests
  {
    private static IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();

      return providerContainer;
    }

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void RenderWebControlPlaceHolderValid()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new PlaceHolderRenderHandler(null));

      IRenderContent renderContent = new TestRenderContent(placeHolder.ToMarkup());

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, providerContainer);

      WriteOutput(processedRenderContent.Content);
      Assert.IsTrue(processedRenderContent.Content.Equals("Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!"));
    }

    [TestMethod]
    public void RenderWebControlPlaceHolderValidAppendRenderContent()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new PlaceHolderRenderHandler(new List<IRenderHandler> { new AppendRenderHandler("APPENDED RENDER HANDLER CONTENT!!!") }));

      IRenderContent renderContent = new TestRenderContent(placeHolder.ToMarkup());

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, providerContainer);

      WriteOutput(processedRenderContent.Content);
      Assert.IsTrue(processedRenderContent.Content.Equals("Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!" + "APPENDED RENDER HANDLER CONTENT!!!"));
    }

    [TestMethod]
    public void RenderNullContent()
    {
      IProviderContainer providerContainer = InitializeProviderContainer();

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new PlaceHolderRenderHandler(null));

      IRenderContent renderContent = new TestRenderContent(null);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, providerContainer);

      WriteOutput(processedRenderContent.Content);
      Assert.IsTrue(processedRenderContent.Content == string.Empty);
    }

    [TestMethod]
    public void RenderEmptyContent()
    {
      IProviderContainer providerContainer = InitializeProviderContainer();

      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new PlaceHolderRenderHandler(null));

      IRenderContent renderContent = new TestRenderContent(null);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, providerContainer);

      WriteOutput(processedRenderContent.Content);
      Assert.IsTrue(processedRenderContent.Content == string.Empty);
    }
  }
}
