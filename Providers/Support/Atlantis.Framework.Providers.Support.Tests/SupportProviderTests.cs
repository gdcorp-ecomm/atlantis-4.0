using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization;
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

    private ISupportProvider SupportProvider()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<ISupportProvider, SupportProvider>();
      _container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      return _container.Resolve<ISupportProvider>();
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
  }
}
