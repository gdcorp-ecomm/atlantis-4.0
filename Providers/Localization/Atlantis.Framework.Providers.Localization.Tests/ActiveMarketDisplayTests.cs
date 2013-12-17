using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.MockImpl;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Tests.Mocks;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Atlantis.Framework.Localization.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.MockImpl.dll")]
  public class ActiveMarketDisplayTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();

    #region Setup

    public const string afeConfig = "atlantis.internationalization.redirect.provider.tests.config";

    [ClassInitialize]
    public static void ClassInit(TestContext c)
    {
      Atlantis.Framework.Engine.Engine.ReloadConfig(afeConfig);

      ReloadCache();
    }

    private static void ReloadCache()
    {
      foreach (var config in Engine.Engine.GetConfigElements())
      {
        DataCache.DataCache.ClearCachedData(config.RequestType);
      }
    }

    private HttpRequest SetContext(string url)
    {
      var request = new MockCustomHttpRequest(url);

      MockHttpContext.SetFromWorkerRequest(request);

      //  Set testing resources (put in HttpContext.Current.Items because the MockImpl Localization triplet classes look there)
      HttpContext.Current.Items[MockLocalizationSettings.CountrySiteMarketMappingsTable] = Properties.Resources.CountrySiteMarketMappings;
      HttpContext.Current.Items[MockLocalizationSettings.CountrySitesActiveTable] = Properties.Resources.CountrySitesActive;
      HttpContext.Current.Items[MockLocalizationSettings.MarketsActiveTable] = Properties.Resources.MarketsActive;

      return HttpContext.Current.Request;
    }

    #endregion

    private IActiveMarketDisplayProvider ActiveMarketDisplayProvider()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IActiveMarketDisplayProvider, ActiveMarketDisplayProvider>();
      _container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      return _container.Resolve<IActiveMarketDisplayProvider>();
    }

    #region IActiveMarketDisplayProvider.GetActiveMarketDisplay Tests

    [TestMethod]
    public void ActiveMarketDisplaySuccess()
    {
      SetContext("http://www.godaddy.com");
      IActiveMarketDisplayProvider provider = ActiveMarketDisplayProvider();
      IList<IActiveMarketDisplay> activeMarketDisplays = provider.GetActiveMarketDisplay(true);
      Assert.AreEqual(true, activeMarketDisplays != null);
      Assert.AreEqual(true, activeMarketDisplays.Count > 0);
      Assert.AreEqual(true, activeMarketDisplays[0].CountryName != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].CountrySiteId != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].Language != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].MarketDescription != string.Empty);
      Assert.AreEqual(true, activeMarketDisplays[0].MarketId != string.Empty);
    }

    [TestMethod]
    public void ActiveMarketDisplay_PublicOnly()
    {
      SetContext("http://www.godaddy.com");
      IActiveMarketDisplayProvider provider = ActiveMarketDisplayProvider();
      IList<IActiveMarketDisplay> activeMarketDisplays = provider.GetActiveMarketDisplay(false);
      Assert.IsTrue(NoRecordsForInternalMappings(activeMarketDisplays));
      Assert.IsTrue(NoRecordsForInternalMarkets(activeMarketDisplays));
      Assert.IsTrue(NoRecordsForInternalCountrySites(activeMarketDisplays));
      Assert.IsTrue(NoDuplicates(activeMarketDisplays));      
    }

    private bool IdenticalMappings(IList<IActiveMarketDisplay> activeMarketDisplays, IList<IActiveMarketDisplay> activeMarketDisplays2)
    {
      bool result = true;

      if (activeMarketDisplays.Count() != activeMarketDisplays2.Count())
      {
        result = false;
      }
      else
      {
        List<IActiveMarketDisplay> amd2List = activeMarketDisplays2.ToList();
        for (int i=0; i < activeMarketDisplays.Count(); i++)
        {
          IActiveMarketDisplay amd = activeMarketDisplays[i];
          
        }
      }

      return result;
    }

    private bool NoRecordsForInternalCountrySites(IList<IActiveMarketDisplay> activeMarketDisplays)
    {

      return !activeMarketDisplays.Any(amd => "rc".Equals(amd.CountrySiteId, StringComparison.OrdinalIgnoreCase));
    }

    private bool NoRecordsForInternalMarkets(IList<IActiveMarketDisplay> activeMarketDisplays)
    {
      return !activeMarketDisplays.Any(amd => "00-00".Equals(amd.MarketId, StringComparison.OrdinalIgnoreCase));
    }

    private bool NoRecordsForInternalMappings(IList<IActiveMarketDisplay> activeMarketDisplays)
    {
      return !activeMarketDisplays.Any(amd => "01-01".Equals(amd.MarketId, StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void ActiveMarketDisplay_IncludeInternalOnly()
    {
      SetContext("http://www.godaddy.com");
      IActiveMarketDisplayProvider provider = ActiveMarketDisplayProvider();
      IList<IActiveMarketDisplay> activeMarketDisplays = provider.GetActiveMarketDisplay(true);
      Assert.IsFalse(NoRecordsForInternalMappings(activeMarketDisplays));
      Assert.IsFalse(NoRecordsForInternalMarkets(activeMarketDisplays));
      Assert.IsFalse(NoRecordsForInternalCountrySites(activeMarketDisplays));
      Assert.IsTrue(NoDuplicates(activeMarketDisplays));
    }

    private bool NoDuplicates(IList<IActiveMarketDisplay> activeMarketDisplays)
    {
      Dictionary<string, IActiveMarketDisplay> distinctList = new Dictionary<string, IActiveMarketDisplay>(StringComparer.OrdinalIgnoreCase);
      foreach (IActiveMarketDisplay amd in activeMarketDisplays)
      {
        if (distinctList.ContainsKey(amd.MarketDescription))
        {
          return false;
        }
        distinctList.Add(amd.MarketDescription, amd);
      }
      return true;
    }

    #endregion

    #region ILocalizationProvider.GetMappedMarketsByCountrySite Tests

    [TestMethod]
    public void GetMappedMarketsByCountrySite_ValidCountrySite_PublicOnly()
    {
      SetContext("http://www.godaddy.com");
      var container = new MockProviderContainer();
      container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();
      IEnumerable<IMarket> mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("www", false);
      Assert.AreEqual(2, mappedMarkets.Count());
      Assert.IsFalse(mappedMarkets.Any(
        mm =>
          !mm.Id.Equals("en-us", StringComparison.OrdinalIgnoreCase) &&
          !mm.Id.Equals("es-US", StringComparison.OrdinalIgnoreCase)));

      //  Internal countrysite
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("rc", false);
      Assert.AreEqual(0, mappedMarkets.Count());

      //  Public countrysite but internal market and mapping
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("00", false);
      Assert.AreEqual(0, mappedMarkets.Count());

      //  Public countrysite and market but internal mapping
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("01", false);
      Assert.AreEqual(0, mappedMarkets.Count());
    }

    [TestMethod]
    public void GetMappedMarketsByCountrySite_ValidCountrySite_IncludeInternalOnly()
    {
      SetContext("http://www.godaddy.com");
      var container = new MockProviderContainer();
      container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();
      IEnumerable<IMarket> mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("www", true);
      Assert.AreEqual(5, mappedMarkets.Count());
      Assert.IsFalse(mappedMarkets.Any(
        mm =>
          !mm.Id.Equals("en-us", StringComparison.OrdinalIgnoreCase) &&
          !mm.Id.Equals("es-us", StringComparison.OrdinalIgnoreCase) &&
          !mm.Id.Equals("qa-qa", StringComparison.OrdinalIgnoreCase) &&
          !mm.Id.Equals("qa-ps", StringComparison.OrdinalIgnoreCase) &&
          !mm.Id.Equals("qa-pz", StringComparison.OrdinalIgnoreCase)));

      //  Internal countrysite
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("rc", true);
      Assert.AreEqual(1, mappedMarkets.Count());
      Assert.AreEqual("rc-rc", mappedMarkets.First().Id.ToLowerInvariant());

      //  Public countrysite but internal market and mapping
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("00", true);
      Assert.AreEqual(1, mappedMarkets.Count());
      Assert.AreEqual("00-00", mappedMarkets.First().Id.ToLowerInvariant());

      //  Public countrysite and market but internal mapping
      mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("01", true);
      Assert.AreEqual(1, mappedMarkets.Count());
      Assert.AreEqual("01-01", mappedMarkets.First().Id.ToLowerInvariant());
    }

    [TestMethod]
    public void GetMappedMarketsByCountrySite_InternalOnlyCountrySite_PublicOnly()
    {
      SetContext("http://www.godaddy.com");
      var container = new MockProviderContainer();
      container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();
      IEnumerable<IMarket> mappedMarkets = localizationProvider.GetMappedMarketsForCountrySite("rc", false);
      Assert.AreEqual(0, mappedMarkets.Count());
    }

    #endregion
  }
}
