using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    public void AppNameIsWrong()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }

    [TestMethod]
    public void RelativePathIsNull()
    {
      string appName = "blah blah";
      string relativePath = null;
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }

    [TestMethod]
    public void UnknownConditions()
    {
      string appName = "sales";
      string relativePath = "/hosting/unknownconditions";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }
    
    [TestMethod]
    public void KnownConditionsWith1True()
    {
      string appName = "sales";
      string relativePath = "/hosting/knownconditions";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsTrue(redirectResult.RedirectRequired);
      Assert.IsNotNull(redirectResult.RedirectData);
      Assert.IsNotNull(redirectResult.RedirectData.Type);
      Assert.IsNotNull(redirectResult.RedirectData.Location);
    }

    [TestMethod]
    public void MultipleConditionsWith1True()
    {
      string appName = "sales";
      string relativePath = "/hosting/multipleconditions";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsTrue(redirectResult.RedirectRequired);
      Assert.IsNotNull(redirectResult.RedirectData);
      Assert.IsNotNull(redirectResult.RedirectData.Type);
      Assert.IsNotNull(redirectResult.RedirectData.Location);
    }

    [TestMethod]
    public void MultipleConditionsAllFalse()
    {
      string appName = "sales";
      string relativePath = "/hosting/multipleconditions-allfalse";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IRedirectResult redirectResult = provider.CheckRedirectRules(appName, relativePath);
      Assert.IsFalse(redirectResult.RedirectRequired);
      Assert.IsNull(redirectResult.RedirectData);
    }
  }
}
