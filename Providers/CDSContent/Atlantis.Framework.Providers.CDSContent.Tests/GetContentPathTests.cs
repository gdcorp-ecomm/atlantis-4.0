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
  public class GetContentPathTests
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
          mockContainer.SetData("IsRequestInternal", true);
          mockContainer.SetData("ServerLocation", ServerLocationType.Dev);
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
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
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
    public void AppNameIsWrong_GetContentPathTests()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}/{1}", appName, relativePath));
    }

    [TestMethod]
    public void RelativePathIsNull_GetContentPathTests()
    {
      string appName = "blah blah";
      string relativePath = "temp";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}/{1}", appName, relativePath));
    }

    [TestMethod]
    public void AllFalse_GetContentPathTests()
    {
      string appName = "sales/unittest";
      string relativePath = "allfalse_getcontentpathtests";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}/{1}", appName, relativePath));
    }

    [TestMethod]
    public void FirstTrue_GetContentPathTests()
    {
      string appName = "sales/unittest";
      string relativePath = "firsttrue_getcontentpathtests";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}", "sales/lp/firsttrue"));
    }

    [TestMethod]
    public void OnlyTrue_GetContentPathTests()
    {
      string appName = "sales/unittest";
      string relativePath = "onlytrue_getcontentathtests";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}", "sales/lp/onlytrue"));
    }

    [TestMethod]
    public void LiteralTrue_GetContentPathTests()
    {
      string appName = "sales/unittest";
      string relativePath = "literaltrue_getcontentpathtests";
      CDSContentProvider provider = (CDSContentProvider) ProviderContainer.Resolve<ICDSContentProvider>();
      string path = provider.GetContentPath(appName, relativePath);
      Assert.IsTrue(path == string.Format("content/{0}", "sales/lp/literaltrue"));
    }
  }
}
