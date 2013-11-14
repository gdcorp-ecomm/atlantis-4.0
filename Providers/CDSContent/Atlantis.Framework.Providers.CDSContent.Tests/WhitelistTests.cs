using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class WhitelistTests
  {

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
          _providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
          _providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();

          MockProviderContainer mockContainer = _providerContainer as MockProviderContainer;
          mockContainer.SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);
          mockContainer.SetMockSetting(MockSiteContextSettings.ServerLocation, ServerLocationType.Dev);
        }

        return _providerContainer;
      }
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void AppNameIsWrong_WhitelistTests()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsNotNull(whiteListResult.UrlData);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void RelativePathIsNull_WhitelistTests()
    {
      string appName = "blah blah";
      string relativePath = null;
      
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      
      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsNotNull(whiteListResult.UrlData);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void StyleIsCorrect_WhitelistTests()
    {
      string appName = "sales/unittest";
      string relativePath = "/default.aspx";
      
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      
      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Widget);
    }

    [TestMethod]
    public void StyleIsIncorrect_WhitelistTests()
    {
      string appName = "sales/unittest";
      string relativePath = "/hosting/email-hosting";
      
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      
      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsFalse(whiteListResult.UrlData["style"] == DocumentStyles.Widget);
    }

    [TestMethod]
    public void BadJson_WhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistbadjson";
      string relativePath = "home";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void Empty_WhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistempty";
      string relativePath = "home";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void ItemNotFound_WhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistoneitem";
      string relativePath = "home2";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsFalse(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void ItemFound_WhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistoneitem";
      string relativePath = "home";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData["style"] == DocumentStyles.FlatPage);
    }
  }
}
