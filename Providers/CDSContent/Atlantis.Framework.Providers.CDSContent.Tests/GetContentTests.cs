using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.CDSContent.Tests.RenderHandlers;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class GetContentTests
  {
    private IList<IRenderHandler> _renderHandlers = new List<IRenderHandler>(8);

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
          _providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();
          _providerContainer.RegisterProvider<IRenderPipelineProvider,RenderPipelineProvider>();

          MockProviderContainer mockContainer = _providerContainer as MockProviderContainer;
          mockContainer.SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);
          mockContainer.SetMockSetting(MockSiteContextSettings.ServerLocation, ServerLocationType.Dev);
        }

        return _providerContainer;
      }
    }

    private void RegisterConditions()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    private void RegisterTokens()
    {
      TokenManager.RegisterTokenHandler(new DataCenterToken());
    }

    private void RegisterRenderHandlers()
    {
      _renderHandlers.Add(new ConditionRenderHandler());
      _renderHandlers.Add(new TargetedMessageRenderHandler());
      _renderHandlers.Add(new TokenRenderHandler());
    }

    private void SetupHttpContext()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    private void ApplicationStart()
    {
      SetupHttpContext();
      RegisterConditions();
      RegisterRenderHandlers();
      RegisterTokens();
    }

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void AppNameIsWrong_GetContentTests()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent = provider.GetContent(appName, relativePath);
      Assert.IsTrue(renderContent.Content == string.Empty);
    }

    [TestMethod]
    public void RelativePathIsNull_GetContentTests()
    {
      string appName = "blah blah";
      string relativePath = null;
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent= provider.GetContent(appName, relativePath);
      Assert.IsTrue(renderContent.Content == string.Empty);
    }

    [TestMethod]
    public void DefaultContentPath_GetContentTests()
    {
      string appName = "sales/unittest";

      string relativePath = "defaultcontentpath_getcontenttests";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent = provider.GetContent(appName, relativePath);
      Assert.IsTrue(renderContent.Content.Contains("Current DataCenter: [@T[dataCenter:name]@T]"));
      Assert.IsTrue(renderContent.Content.Contains("eastern hemisphere"));
      Assert.IsTrue(renderContent.Content.Contains("[@TargetedMessage[imageUrl]@TargetedMessage]"));
    }

    [TestMethod]
    public void DraftVersion_GetContentTests()
    {
      string appName = "sales";

      string relativePath = "yermom?version=51c23725f778fc10204fd85d";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent = provider.GetContent(appName, relativePath);
      Assert.IsTrue(!string.IsNullOrEmpty(renderContent.Content));
    }

    [TestMethod]
    public void ContentNotFound_GetContentTests()
    {
      string appName = "sales/unittest";
      string relativePath = "notfound-notfound-notfound";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent = provider.GetContent(appName, relativePath);
      Assert.IsTrue(renderContent.Content == string.Empty);
    }

    [TestMethod]
    public void RenderPipelineTests_GetContentTests()
    {
      string appName = "sales/unittest";
      string relativePath = "renderpipelinetests_getcontenttests";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRenderContent renderContent = provider.GetContent(appName, relativePath);
      Assert.IsTrue(renderContent.Content.Contains("Current DataCenter: [@T[dataCenter:name]@T]"));
      Assert.IsTrue(renderContent.Content.Contains("eastern hemisphere"));
      Assert.IsTrue(renderContent.Content.Contains("[@TargetedMessage[imageUrl]@TargetedMessage]"));
    }
       
  }
}
