using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackageStateProvider;
using Atlantis.Framework.Providers.DomainSearch;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Providers.DomainProductPackage.Test
{
  [TestClass]
  [DeploymentItem("App.config")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainSearch.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainProductPackage.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  public class PrivacyProductPackageTest
  {
    private const string SOURCE_CODE = "mblDPPSearch";

    [TestInitialize]
    public void InitializeTests()
    {
      MockHttpRequest request = new MockHttpRequest("http://spoonymac.com/");
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
          ((MockProviderContainer)_providerContainer).SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IProxyContext, WebProxyContext>();
          _providerContainer.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
          _providerContainer.RegisterProvider<IDomainSearchProvider, DomainSearchProvider>();
          _providerContainer.RegisterProvider<IDomainProductPackageProvider, DomainProductPackageProvider>();
          _providerContainer.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
          _providerContainer.RegisterProvider<ICurrencyProvider, Currency.CurrencyProvider>();
          _providerContainer.RegisterProvider<IProductPackageItemProvider, ProductPackageItemProvider>();
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

    private IDomainProductPackageProvider _domainProductPackageProvider;
    private IDomainProductPackageProvider DomainProductPackageProvider
    {
      get
      {
        if (_domainProductPackageProvider == null)
        {
          _domainProductPackageProvider = ProviderContainer.Resolve<IDomainProductPackageProvider>();
        }

        return _domainProductPackageProvider;
      }
    }

    [TestMethod]
    public void PrivacyChildTest()
    {
       var domains = DomainSearch.SearchDomain("spoonymac-rastaman.com", SOURCE_CODE, string.Empty);

      var domainProductPackages = DomainProductPackageProvider.BuildDomainProductPackageGroups(domains.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      // Get a package to add the privacy child.
      var domainProductPackage = domainProductPackages.ToList()[0];

      // Create a privacy package item.
      //domainProductPackage.SetPrivacy(ProductPackageItemNames.PRIVACY);

      // Save package to session
      var storeProvider = ProviderContainer.Resolve<IPersistanceStoreProvider>();
      storeProvider.SaveDomainProductPackages(domainProductPackages); 

      // Get package from session
      Assert.IsTrue(storeProvider.TryGetDomainProductPackages(out domainProductPackages));


      

    }

  }
}
