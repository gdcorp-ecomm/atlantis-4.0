﻿using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

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
          _providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();
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
    public void AppNameIsWrong()
    {
      string appName = "blah blah";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsFalse(whiteListResult.Exists);
      Assert.IsNull(whiteListResult.UrlData);
    }

    [TestMethod]
    public void RelativePathIsNull()
    {
      string appName = "blah blah";
      string relativePath = null;
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsFalse(whiteListResult.Exists);
      Assert.IsNull(whiteListResult.UrlData);
    }

    [TestMethod]
    public void UrlExists()
    {
      string appName = "sales";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsTrue(whiteListResult.Exists);
    }

    [TestMethod]
    public void UrlDoesNotExist()
    {
      string appName = "sales";
      string relativePath = "hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsFalse(whiteListResult.Exists);
    }

    [TestMethod]
    public void StyleIsCorrect()
    {
      string appName = "sales";
      string relativePath = "/default.aspx";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsTrue(whiteListResult.UrlData.Style == "w");
    }

    public void StyleIsIncorrect()
    {
      string appName = "sales";
      string relativePath = "/hosting/email-hosting";
      ICDSContentProvider provider = ProviderContainer.Resolve<ICDSContentProvider>();
      IWhitelistResult whiteListResult = provider.CheckWhiteList(appName, relativePath);
      Assert.IsTrue(whiteListResult.Exists);
      Assert.IsFalse(whiteListResult.UrlData.Style == "w");
    }
  }
}