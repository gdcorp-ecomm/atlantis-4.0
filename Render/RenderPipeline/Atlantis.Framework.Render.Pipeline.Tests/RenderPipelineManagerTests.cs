using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Pipeline.Tests.RenderContent;
using Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers;
using Atlantis.Framework.Render.Pipeline.Tests.Tokens;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.Pipeline.Tests
{
  [TestClass]
  public class RenderPipelineManagerTests
  {
    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get { return _objectProviderContainer ?? (_objectProviderContainer = new ObjectProviderContainer()); }
    }

    private void WriteOutput(string output)
    {
      #if DEBUG
      Debug.WriteLine(output);
      #else
      Console.WriteLine(output);
      #endif
    }

    private void SetupHttpContext()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    private void RegisterProviders()
    {
      ObjectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ObjectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ObjectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
    }

    private void RegisterConditions()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    private void RegisterTokens()
    {
      TokenManager.RegisterTokenHandler(new DataCenterToken());
    }

    private void ApplicationStart()
    {
      SetupHttpContext();
      RegisterProviders();
      RegisterConditions();
      RegisterTokens();
    }    

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void RenderContentEmptyPipelineTest()
    {
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();

      IRenderContent renderContent = new SimpleRenderContent();
      renderContent.Content = "Hello World!";

      renderPipelineManager.RenderContent(renderContent, ObjectProviderContainer);

      WriteOutput(renderContent.Content);
      Assert.IsTrue(renderContent.Content == "Hello World!");
    }

    [TestMethod]
    public void RenderContentOrderOfPipelineTest()
    {
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new RenderHandlerThree());
      renderPipelineManager.AddRenderHandler(new RenderHandlerOne());
      renderPipelineManager.AddRenderHandler(new RenderHandlerTwo());

      IRenderContent renderContent = new SimpleRenderContent();
      renderContent.Content = string.Empty;

      renderPipelineManager.RenderContent(renderContent, ObjectProviderContainer);

      WriteOutput(renderContent.Content);
      Assert.IsTrue(renderContent.Content == "three one two");
    }

    [TestMethod]
    public void RenderContentWithAllVariationsIntegrationTest()
    {
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new ConditionRenderHandler());
      renderPipelineManager.AddRenderHandler(new TargetedMessageRenderHandler());
      renderPipelineManager.AddRenderHandler(new TokenRenderHandler());

      IRenderContent renderContent = new SimpleRenderContent();
      renderContent.Content = @"<div>Current DataCenter: [@T[dataCenter:name]@T]</div>
                                ##if(dataCenter(AP))
                                <div>Deliver amazing performance to the eastern hemisphere</div>
                                ##else
                                <div>Deliver amazing performance to the western hemisphere</div>
                                ##endif
                        
                                ##if(targetedMessageId(1234)) 
                                <div>[@TargetedMessage[message]@TargetedMessage]</div>
                                <div><img alt=""targeted message"" src=""[@TargetedMessage[imageUrl]@TargetedMessage]"" />
                                ##endif";

      renderPipelineManager.RenderContent(renderContent, ObjectProviderContainer);

      WriteOutput(renderContent.Content);

      Assert.IsTrue(renderContent.Content.Contains("Asia Pacific"));
      Assert.IsTrue(renderContent.Content.Contains("eastern hemisphere"));
      Assert.IsTrue(renderContent.Content.Contains("Targeted Message Here!!!!"));
    }

    [TestMethod]
    public void RenderContentHtmlFileIntegrationTest()
    {
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new ConditionRenderHandler());
      renderPipelineManager.AddRenderHandler(new TargetedMessageRenderHandler());
      renderPipelineManager.AddRenderHandler(new TokenRenderHandler());

      string withExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Render.Pipeline.Tests.Data.home-page-with-conditions.html")))
      {
        withExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      IRenderContent renderContent = new SimpleRenderContent();
      renderContent.Content = withExpressionsMarkup;

      double startNanoSeconds = DateTime.UtcNow.Ticks;

      renderPipelineManager.RenderContent(renderContent, ObjectProviderContainer);

      double endNanoSeconds = DateTime.UtcNow.Ticks;

      double totalMilliSeconds = (endNanoSeconds - startNanoSeconds) / 10000.00;

      WriteOutput(string.Format("Total Render Time: {0} milliseconds", totalMilliSeconds));
      WriteOutput(renderContent.Content);

      Assert.IsTrue(renderContent.Content.Contains("Asia Pacific"));
      Assert.IsTrue(renderContent.Content.Contains("Targeted Message Here!!!!"));
    }
  }
}
