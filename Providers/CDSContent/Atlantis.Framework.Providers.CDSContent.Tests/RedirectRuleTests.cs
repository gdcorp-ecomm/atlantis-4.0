using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Containers;
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
  public class RedirectRuleTests
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

    private void RegisterTokens()
    {
      TokenProvider.RegisterTokenHandler(new DataCenterToken());
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
      RegisterTokens();
    }    

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void AppNameIsWrong_RedirectRuleTests()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }

    [TestMethod]
    public void RelativePathIsNull_RedirectRuleTests()
    {
      string appName = "blah blah";
      string relativePath = null;
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }

    [TestMethod]
    public void AllFalse_RedirectRuleTests()
    {
      string appName = "sales/unittest";
      string relativePath = "allfalse_redirectruletests";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }
    
    [TestMethod]
    public void FirstTrue_RedirectRuleTests()
    {
      string appName = "sales/unittest";
      string relativePath = "firsttrue_redirectruletests";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsTrue(redirectResult.RedirectRequired);
      Assert.IsNotNull(redirectResult.RedirectData);
      Assert.IsNotNull(redirectResult.RedirectData.Type);
      Assert.IsNotNull(redirectResult.RedirectData.Location);
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("firsttrue"));
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("Asia Pacific"));
    }
    
    [TestMethod]
    public void OnlyTrue_RedirectRuleTests()
    {
      string appName = "sales/unittest";
      string relativePath = "onlytrue_redirectruletests.rule?docid=51a8dd00f778fc17c8500add";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsTrue(redirectResult.RedirectRequired);
      Assert.IsNotNull(redirectResult.RedirectData);
      Assert.IsNotNull(redirectResult.RedirectData.Type);
      Assert.IsNotNull(redirectResult.RedirectData.Location);
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("onlytrue"));
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("Asia Pacific"));
    }

    [TestMethod]
    public void LiteralTrue_RedirectRuleTests()
    {
      string appName = "sales/unittest";
      string relativePath = "literaltrue_redirectruletests";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsTrue(redirectResult.RedirectRequired);
      Assert.IsNotNull(redirectResult.RedirectData);
      Assert.IsNotNull(redirectResult.RedirectData.Type);
      Assert.IsNotNull(redirectResult.RedirectData.Location);
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("literaltrue"));
      Assert.IsTrue(redirectResult.RedirectData.Location.Contains("Asia Pacific"));
    }
  }
}
