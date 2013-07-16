using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainSearch.dll")]
  public class DomainSearchTests
  {
    private const string SOURCE_CODE = "mblDPPSearch";

    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://spoonymac.com");
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
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IProxyContext, ProxyContext.WebProxyContext>();
          _providerContainer.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
          _providerContainer.RegisterProvider<IDomainSearchProvider, DomainSearchProvider>();
        }

        return _providerContainer;
      }
    }

    private IDomainSearchProvider _domainSearch;
    private IDomainSearchProvider DomainSearch
    {
      get
      {
        if (_domainSearch == null)
        {
          _domainSearch = ProviderContainer.Resolve<IDomainSearchProvider>();
        }

        return _domainSearch;
      }
    }

    [TestMethod]
    public void DomainSearchResultTest()
    {
      const string searchPhrase = "spoonymac.com";

      IDomainSearchResult domainSearchResult;

      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);
     
      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.AllDomains.Count != 0);
    }

    [TestMethod]
    public void DomainSearchExactMatchTest()
    {
      const string searchPhrase = "spoonymac.com";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;

      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);
     
      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.EXACT_MATCH, out domains));
      Assert.IsTrue(domains.Any(d => d.Domain.DomainName.ToLowerInvariant() == searchPhrase));
    }

    [TestMethod]
    public void DomainSearchExactMatchAvailableTest()
    {
      const string searchPhrase = "spoonymacou812.com";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.EXACT_MATCH, out domains));
      var isAvailable = domains.Any(d => d.IsAvailable && d.Domain.DomainName == searchPhrase);
      Assert.IsTrue(isAvailable);
    }

    [TestMethod]
    public void DomainSearchMultipleExactMatchAvailableTest()
    {
      const string searchPhrase = "spoonymacou812.com,maseratii8a4re.net";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.EXACT_MATCH, out domains));

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

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.EXACT_MATCH, out domains));

      var searchPhrase1 = domains.Any(d => !d.IsAvailable && d.Domain.DomainName == searchPhrase);

      Assert.IsTrue(searchPhrase1);
    }

    [TestMethod]
    public void DomainSearchPremiumTest()
    {
      const string searchPhrase = "easy-domain-premium-test.com";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.PREMIUM, out domains));

      var hasAvailblePremiums = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.PREMIUM);

      Assert.IsTrue(hasAvailblePremiums);
    }

    [TestMethod]
    public void DomainSearchSimiliarTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline.com";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.SIMILIAR, out domains));

      var hasSimiliar = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.SIMILIAR);

      Assert.IsTrue(hasSimiliar);
    }

    [TestMethod]
    public void DomainSearchAffixTest()
    {
      const string searchPhrase = "dogs-cats-canine";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.AFFIX, out domains));

      var hasAffix = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.AFFIX);

      Assert.IsTrue(hasAffix);
    }

    [TestMethod]
    public void DomainSearchCcTldTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline.uk";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.COUNTRY_CODE_TLD, out domains));

      var hasCcTld = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.COUNTRY_CODE_TLD);

      Assert.IsTrue(hasCcTld);
    }


    // FAILS - NO Privates as of initial unit testing
    [TestMethod]
    public void DomainSearchPrivateTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.PRIVATE, out domains));

      var hasPrivates = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.PRIVATE);

      Assert.IsTrue(hasPrivates);
    }

     // FAILS - NO Auction as of initial unit testing
    [TestMethod]
    public void DomainSearchAuctionTest()
    {
      const string searchPhrase = "dogs-cats-canine-feline";

      IDomainSearchResult domainSearchResult;
      IEnumerable<IFindResponseDomain> domains;
      var success = DomainSearch.SearchDomain(searchPhrase, SOURCE_CODE, string.Empty, out domainSearchResult);

      Assert.IsTrue(success);
      Assert.IsTrue(domainSearchResult.GetDomains(DomainGroupTypes.AUCTIONS, out domains));

      var hasAuctions = domains.Any(d => d.DomainSearchDataBase == DomainGroupTypes.AUCTIONS);

      Assert.IsTrue(hasAuctions);
    }
  }
}
