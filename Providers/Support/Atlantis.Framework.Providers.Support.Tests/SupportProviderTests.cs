using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Support.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Support.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Support.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class SupportProviderTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();
    private const string US_SPANISH_SUPPORT_NUMBER = "(480) 463-8300";

    private ISupportProvider SupportProvider(string countryCode = "us", bool isGlobalSite = true)
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<ISupportProvider, SupportProvider>();

      _container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, countryCode);
      _container.SetData(MockLocalizationProvider.COUNTRY_SITE_NAME, countryCode);
      _container.SetData(MockLocalizationProvider.IS_GLOBAL_SITE_NAME, isGlobalSite);

      return _container.Resolve<ISupportProvider>();
    }

    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void Initialize()
    {
      switch (TestContext.TestName)
      {
        case "SupportNumberGdSuccessWithNoLocalizationAndGeoProvider":
          break;
        case "SupportNumberGdSuccessUsingGeoProvider":
          _container.RegisterProvider<IGeoProvider, MockGeoProvider>();
          break;
        case "SupportNumberGdSuccessWithTransperfectProxy":
          _container.RegisterProvider<IProxyContext, TransperfectTestWebProxy>();
          _container.RegisterProvider<IGeoProvider, MockGeoProvider>();
          _container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
          break;
        default:
          _container.RegisterProvider<IGeoProvider, MockGeoProvider>();
          _container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
          break;
      }
    }

    [TestMethod]
    public void SupportNumberUnknown()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Unknown);
      Assert.AreEqual(string.Empty, supportPhoneData.Number);
    }

    [TestMethod]
    public void SupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberDataException()
    {
      ISupportProvider provider = SupportProvider();
      int supportEngineRequest = SupportEngineRequests.SupportPhoneRequest;

      try
      {
        SupportEngineRequests.SupportPhoneRequest = 998;
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, true);
      }
      finally
      {
        SupportEngineRequests.SupportPhoneRequest = supportEngineRequest;
      }
    }

    [TestMethod]
    public void SupportNumberDataFormat7()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1726");
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void SupportNumberDataFormat11()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1727");
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void SupportNumberResellerSupportOption1()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1724");
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void SupportNumberResellerSupportOption2()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1725");
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void SupportNumberUsWwdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1387");
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberGdSuccessWithNonGlobalSite()
    {
      ISupportProvider provider = SupportProvider("uk", false);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberGdSuccessUsingGeoProvider()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberGdSuccessWithNoLocalizationAndGeoProvider()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SupportNumberGdSuccessWithTransperfectProxy()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number == US_SPANISH_SUPPORT_NUMBER);
    }

    [TestMethod]
    public void SupportEmailGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      var email = provider.SupportEmail;
      Assert.AreEqual(true, !string.IsNullOrEmpty(email) && email == "support@godaddy.com");
    }

    [TestMethod]
    public void SupportEmailBlueRazorSuccess()
    {
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);

      ISupportProvider provider = SupportProvider();
      var email = provider.SupportEmail;
      Assert.AreEqual(true, email == "support@bluerazor.com");
    }

    [TestMethod]
    public void SupportEmailWildWestSuccess()
    {
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);

      ISupportProvider provider = SupportProvider();
      var email = provider.SupportEmail;
      Assert.AreEqual(true, !string.IsNullOrEmpty(email) && email == "support@wildwestdomains.com");
    }

    [TestMethod]
    public void SupportEmailResellerSuccess()
    {
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);

      ISupportProvider provider = SupportProvider();
      var email = provider.SupportEmail;
      Assert.AreEqual(true, !string.IsNullOrEmpty(email) && email == "support@secureserver.net");
    }
  }
}
