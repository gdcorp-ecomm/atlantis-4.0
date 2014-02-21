using System;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Logging.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  [TestClass]
  [DeploymentItem("App.config")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainSearch.dll")]
  public class DomainSearchTests
  {
    private const string SOURCE_CODE = "mblDPPSearch";
    private const string _LOGDOMAINSEARCHTRAFFICAPPSETTING = "ATLANTIS.LOGDOMAINSEARCHTRAFFIC";

    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.SetData(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IProxyContext, WebProxyContext>();
          _providerContainer.RegisterProvider<ILocalizationProvider, LocalizationProviderTestProxy>();
          _providerContainer.RegisterProvider<IDomainSearchProvider, DomainSearchProvider>();
          _providerContainer.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();
        }

        return _providerContainer;
      }
    }

    private IDomainSearchProvider _domainSearch;
    private IDomainSearchProvider DomainSearch
    {
      get { return _domainSearch ?? (_domainSearch = ProviderContainer.Resolve<IDomainSearchProvider>()); }
    }

    [TestMethod]
    public void DomainSearchResultTest()
    {
      const string searchPhrase = "spoonymac.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);
      Assert.IsTrue(domainSearchResult.FindResponseDomains.Count != 0);
    }

    [TestMethod]
    public void DomainSearchExactMatchTest()
    {
      ProviderContainer.RegisterProvider<ILogDomainSearchResultsProvider, MockLogDomainSearchResultsProvider>();
      ProviderContainer.SetData("appsetting." + _LOGDOMAINSEARCHTRAFFICAPPSETTING, "true");

      const string searchPhrase = "уичтрдеычиикктуйггфф.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);
      Assert.IsTrue(domains.Any(d => d.Domain.DomainName.ToLowerInvariant() == searchPhrase));
    }

    [TestMethod]
    public void DomainSearchExactMatchNoLogTest()
    {
      const string searchPhrase = "уичтрдеычиикктуйггфф.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);
      Assert.IsTrue(domains.Any(d => d.Domain.DomainName.ToLowerInvariant() == searchPhrase));
    }

    [TestMethod]
    public void DomainSearchExactMatchAvailableTest()
    {
      const string searchPhrase = "УИЧТРДЕЫЧИИККТУЙГГФФ.COM";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);
      var isAvailable = domains.Any(d => d.IsAvailable && (d.Domain.DomainName.Equals(searchPhrase, StringComparison.OrdinalIgnoreCase)));
      Assert.IsTrue(isAvailable);
    }

    [TestMethod]
    public void DomainSearchMultipleExactMatchAvailableTest()
    {
      const string searchPhrase = "spoonymacou812134.com,maseratii8a4re134.net";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);

      Assert.IsTrue(domains.Count() == 2);

      var searchPhrase1 = domains.Any(d => d.IsAvailable && d.Domain.DomainName == "spoonymacou812.com");
      var searchPhrase2 = domains.Any(d => d.IsAvailable && d.Domain.DomainName == "maseratii8a4re.net");

      Assert.IsTrue(searchPhrase1);
      Assert.IsTrue(searchPhrase2);
    }

    [TestMethod]
    public void DomainSearchExactMatchUnavailableTest()
    {
      const string searchPhrase = "google.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);

      var searchPhrase1 = domains.Any(d => !d.IsAvailable && d.Domain.DomainName == searchPhrase);
      Assert.IsTrue(searchPhrase1);
    }

    [TestMethod]
    public void DomainSearchPremiumTest()
    {
      const string searchPhrase = "easy-domain-premium-test.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.PREMIUM);

      var hasAvailblePremiums = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.PREMIUM);
      Assert.IsTrue(hasAvailblePremiums);
    }

    [TestMethod]
    public void DomainSearchSimiliarTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.SIMILIAR);
      var hasSimiliar = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.SIMILIAR);

      Assert.IsTrue(hasSimiliar);
    }

    [TestMethod]
    public void DomainSearchAffixTest()
    {
      const string searchPhrase = "SPOONYMAC-HELLO-WORLD.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.AFFIX);
      var hasAffix = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.AFFIX);

      Assert.IsTrue(hasAffix);
    }

    [TestMethod]
    public void DomainSearchCcTldTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline.uk";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.COUNTRY_CODE_TLD);
      var hasCcTld = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.COUNTRY_CODE_TLD);
      Assert.IsTrue(hasCcTld);
    }

    // FAILS - NO Privates as of initial unit testing
    [TestMethod]
    public void DomainSearchPrivateTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.PRIVATE);
      var hasPrivates = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.PRIVATE);

      Assert.IsTrue(hasPrivates);
    }

    // FAILS - NO Auction as of initial unit testing
    [TestMethod]
    public void DomainSearchAuctionTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.AUCTIONS);
      var hasAuctions = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.AUCTIONS);

      Assert.IsTrue(hasAuctions);
    }

    [TestMethod]
    public void DomainSearchCrossCheckTest()
    {
      const string searchPhrase = "SPOONYMAC-HELLO-WORLD.com";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.CROSS_CHECK);
      var hasCrossCheck = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.CROSS_CHECK);

      Assert.IsTrue(hasCrossCheck);
    }

    [TestMethod]
    public void DomainSearchPreReg()
    {
      const string searchPhrase = "iowa.menu";

      var domainSearchResult = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty);
      Assert.IsTrue(domainSearchResult.IsSuccess);

      var domains = domainSearchResult.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH);

      Assert.IsTrue(domains[0].InPreRegPhase);
      Assert.IsTrue(domains[0].LaunchPhaseItems.Any());
    }
  }
}