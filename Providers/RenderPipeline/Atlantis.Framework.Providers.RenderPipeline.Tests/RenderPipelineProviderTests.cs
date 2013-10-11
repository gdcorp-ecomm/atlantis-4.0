using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Tests.Helpers;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests
{

  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class RenderPipelineProviderTests
  {
    private MockErrorLogger _testLogger = new MockErrorLogger();
    private IErrorLogger _defaultLogger = Engine.EngineLogging.EngineLogger;

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      Engine.EngineLogging.EngineLogger = _testLogger;
    }

    public static IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
      return providerContainer;
    }

    [TestMethod]
    public void RenderContentCallsRenderPipeline()
    {
      string content = @"<div>[@L[app:phrase]@L]</div>";

      int renderHandlerTimesCalled = 0;

      List<IRenderHandler> renderHandlers = new List<IRenderHandler>();

      var renderHandler = new Mock<IRenderHandler>();
      renderHandler.Setup(rh => rh.ProcessContent(It.IsAny<IProcessedRenderContent>(), It.IsAny<IProviderContainer>()))
                   .Callback(() => renderHandlerTimesCalled++);

      renderHandlers.Add(renderHandler.Object);

      IProviderContainer providerContainer = InitializeProviderContainer();

      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);

      renderPipelineProvider.RenderContent(content, renderHandlers);

      Assert.IsTrue(renderHandlerTimesCalled == 1, "Render Handler was not called correctly");
    }

    [TestMethod]
    public void RenderContentNullHandlers()
    {
      string content = @"<div>[@L[app:phrase]@L]</div>";

      bool renderPipelineCalled = false;
     
      IProviderContainer providerContainer = InitializeProviderContainer();

     
      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);

      string finalContent = renderPipelineProvider.RenderContent(content, null);

      Assert.IsFalse(renderPipelineCalled, "Render Pipeline should not be called");
      Assert.IsTrue(content.Equals(finalContent), "Should return the passed in content.");
    }

    [TestMethod]
    public void RenderContentAllNullParameters()
    {
      string content = null;

      IProviderContainer providerContainer = InitializeProviderContainer();

      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);

      string finalContent = renderPipelineProvider.RenderContent(content, null);

      Assert.IsTrue(finalContent == null, "Content should be null");
    }

    [TestMethod]
    public void RenderContentCatchesException()
    {
      _testLogger.Exceptions.Clear();
      string content = @"<div>[@L[app:phrase]@L]</div>";

      List<IRenderHandler> renderHandlers = new List<IRenderHandler>();

      var renderHandler = new Mock<IRenderHandler>();
      renderHandler.Setup(rh => rh.ProcessContent(It.IsAny<IProcessedRenderContent>(), It.IsAny<IProviderContainer>()))
                   .Throws(new Exception("Error"));
      renderHandlers.Add(renderHandler.Object);

      IProviderContainer providerContainer = InitializeProviderContainer();
      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);
      var finalContent = renderPipelineProvider.RenderContent(content, renderHandlers);

      Assert.IsTrue(finalContent.Equals(content), "Method should return passed in content on exception");
      Assert.AreEqual(1, _testLogger.Exceptions.Count,"Should Log Atlantis Exception");
    }

    [TestMethod]
    public void RenderContentPassesAllRenderHandlers()
    {
      string content = @"<div>[@L[app:phrase]@L]</div>";
      int renderHandlerTimesCalled = 0;

      List<IRenderHandler> renderHandlers = new List<IRenderHandler>();
      for (int i = 0; i < 10; i++)
      {
              var renderHandler = new Mock<IRenderHandler>();
      renderHandler.Setup(rh => rh.ProcessContent(It.IsAny<IProcessedRenderContent>(), It.IsAny<IProviderContainer>()))
                   .Callback(() => renderHandlerTimesCalled++);
      }


      IProviderContainer providerContainer = InitializeProviderContainer();
      
      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);

      renderPipelineProvider.RenderContent(content, renderHandlers);

      Assert.IsTrue(renderHandlerTimesCalled == renderHandlers.Count, "Not All Render Handlers passed.");
    }

    [TestMethod]
    public void RenderContentTestRenderHandler()
    {
      string content = @"<div>[@L[app:phrase]@L]</div>";

      List<IRenderHandler> renderHandlers = new List<IRenderHandler>();

      var testRenderHandler = new TestRenderHandler();
      renderHandlers.Add(testRenderHandler);

      IProviderContainer providerContainer = InitializeProviderContainer();

      var renderPipelineProvider = new RenderPipelineProvider(providerContainer);

      var finalContent = renderPipelineProvider.RenderContent(content, renderHandlers);

      Assert.IsTrue(finalContent.Equals("test"),"Content Was not Rendered Correctly");
    }
    
  }
}

