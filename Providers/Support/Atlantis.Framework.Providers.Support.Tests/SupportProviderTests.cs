using System;
using System.ComponentModel;
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
        case "SupportNumberGdSuccessWithEsUsMarket":
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
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void HostingSupportNumberGdFailure()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Hosting);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void HostingSupportNumberBlueRazorEmpty()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Hosting);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void HostingSupportNumberResellerOption3Success()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1828);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Hosting);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }
    [TestMethod]
    public void HostingExchangeSupportNumberResellerOption3Success()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1828);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void HostingExchangeSupportNonGlobalSiteSuccess()
    {
      ISupportProvider provider = SupportProvider("uk", false);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void HostingExchangeSupportNumberResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void HostingExchangeSupportNumberAPIResellerEmpty()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void HostingExchangeSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void HostingExchangeSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.HostingExchange);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void BillingSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Billing);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void BillingSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Billing);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }


    [TestMethod]
    public void BillingSupportNumberWWDSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Billing);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void BillingSupportNumberResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Billing);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void BillingSupportNumberAPIResellerEmpty()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 440354);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Billing);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void CompanyFaxSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyFax);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void CompanyFaxSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyFax);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void CompanyFaxSupportNumberWWDSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyFax);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void CompanyFaxSupportNumberResellerEmpty()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyFax);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void CompanyMainSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void CompanyMainSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void CompanyMainSupportNumberWWDSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void CompanyMainSupportAPIResellerEmpty()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 440354);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void CompanyMainSupportNumberOption1()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1724);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void CompanyMainSupportNumberOption2()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1725);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.CompanyMain);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void DomainsSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void DomainsSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void DomainsSupportNumberWWDSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void DomainsSupportNumberOption1()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1724);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void DomainsSupportNumberOption2()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1725);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }
    [TestMethod]
    public void DomainsSupportAPIResellerEmpty()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 440354);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Domains);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void PremiumDomainsSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.PremiumDomains);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void PremiumDomainsSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.PremiumDomains);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void ServerSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Server);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void ServerSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Server);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void ServerSupportNumberResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Server);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void ServerSupportNumberResellerOption3Success()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1828);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Server);
        Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void ServerSupportNumberWWDEmpty()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Server);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void AdSpaceSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.AdSpace);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }
    [TestMethod]
    public void AdSpaceSupportNumberResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.AdSpace);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void AdSpaceSupportNumberWWDEmpty()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.AdSpace);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void AdSpaceSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.AdSpace);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SSLSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider("uk", false);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.SSL);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void SSLSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.SSL);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void DesignTeamSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.DesignTeam);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void DesignTeamSupportNumberBlueRazorSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.DesignTeam);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void DesignTeamSupportNumberResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 998);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.DesignTeam);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void ResellerSalesSupportNumberSuperResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 441087);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.ResellerSales);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void ResellerTechSupportNumberSuperResellerSuccess()
    {
      ISupportProvider provider = SupportProvider();
      int plDataRequest = DataCache.DataCacheEngineRequests.GetPrivateLabelData;

      try
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = 999;
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 281896);
        ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Technical);
        Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
      }
      finally
      {
        DataCache.DataCacheEngineRequests.GetPrivateLabelData = plDataRequest;
      }
    }

    [TestMethod]
    public void ResellerSalesSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.ResellerSales);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void ResellerSalesSupportNumberBlueRazorEmpty()
    {
      ISupportProvider provider = SupportProvider();
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 2);
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.ResellerSales);
      Assert.AreEqual(true, supportPhoneData.Number == string.Empty);
    }

    [TestMethod]
    public void McafeeSalesSupportNumberGdSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.Mcafee);
      Assert.AreEqual(true, supportPhoneData.Number != string.Empty);
    }

    [TestMethod]
    public void AccountingSupportFaxNumberGSuccess()
    {
      ISupportProvider provider = SupportProvider();
      ISupportPhoneData supportPhoneData = provider.GetSupportPhone(SupportPhoneType.AccountingFax);
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
      catch
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
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1726);
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
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1727);
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
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1724);
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
        _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1725);
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
      _container.SetData<int>(MockSiteContextSettings.PrivateLabelId, 1387);
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
    public void SupportNumberGdSuccessWithBRGlobalSite()
    {
      ISupportProvider provider = SupportProvider("br", false);
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
    public void SupportNumberGdSuccessWithEsUsMarket()
    {
      IMarket marketInfo = new MockMarketInfo("es-US", "Estados Unidos - Español", false, "es-US");
      _container.SetData(MockLocalizationProvider.MARKET_INFO, marketInfo);
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
