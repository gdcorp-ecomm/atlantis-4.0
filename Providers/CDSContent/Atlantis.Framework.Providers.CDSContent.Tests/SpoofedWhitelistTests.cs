using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class SpoofedWhitelistTests
  {
    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get { return _objectProviderContainer ?? (_objectProviderContainer = new ObjectProviderContainer()); }
    }

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

    private void RegisterProviders()
    {
      ObjectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ObjectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ObjectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
    }

    private void SetupHttpContext()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?whitelist=5202d051f778fc20bcb6c12a");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    private void ApplicationStart()
    {
      SetupHttpContext();
      RegisterProviders();
      RegisterConditions();
    }

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void ItemNotFound_SpoofedWhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistoneitem";
      string relativePath = "homexyz2";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsFalse(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData.Style == DocumentStyles.Unknown);
    }

    [TestMethod]
    public void ItemFound_SpoofedWhitelistTests()
    {
      string appName = "atlantis/_unittests/whitelistoneitem";
      string relativePath = "homexyz";

      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);

      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData.Style == DocumentStyles.FlatPage);
    }
  }

}
