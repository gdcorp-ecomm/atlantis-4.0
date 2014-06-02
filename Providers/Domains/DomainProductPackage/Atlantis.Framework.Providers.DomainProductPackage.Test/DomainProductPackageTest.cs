using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;
using Atlantis.Framework.Providers.DomainProductPackage.StateProvider;
using Atlantis.Framework.Providers.DomainSearch;
using Atlantis.Framework.Providers.DomainSearch.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Geo;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.AppSettings;
using System;
using Moq;

namespace Atlantis.Framework.Providers.DomainProductPackage.Test
{
  [TestClass]
  [DeploymentItem("App.config")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]

  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainSearch.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.DomainProductPackage.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  public class DomainProductPackageTest
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
          ((MockProviderContainer) _providerContainer).SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IProxyContext, WebProxyContext>();
          _providerContainer.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
          _providerContainer.RegisterProvider<IDomainSearchProvider, DomainSearchProvider>();
          _providerContainer.RegisterProvider<IDomainProductPackageProvider, DomainProductPackageProvider>();
          _providerContainer.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
          _providerContainer.RegisterProvider<ICurrencyProvider, Currency.CurrencyProvider>();
          _providerContainer.RegisterProvider<IPersistanceStoreProvider, PersistanceStoreProvider>();
          _providerContainer.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
          _providerContainer.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();
          _providerContainer.RegisterProvider<IGeoProvider, GeoProvider>();
          _providerContainer.RegisterProvider<IAppSettingsProvider, AppSettingsProvider>();

        }

        return _providerContainer;
      }
    }

    private IDomainProductPackage _domainProductPackage;

    private IDomainProductPackage DomainProductPackage
    {
        get
        {
            if (_domainProductPackage == null)
            {
                _domainProductPackage = ProviderContainer.Resolve<IDomainProductPackage>();
            }
            return _domainProductPackage;
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

    private IPersistanceStoreProvider _storeProvider;
    private IPersistanceStoreProvider StoreProvider
    {
      get
      {
        if (_storeProvider == null)
        {
          _storeProvider = ProviderContainer.Resolve<IPersistanceStoreProvider>();
        }

        return _storeProvider;
      }
    }

    [TestMethod]
    public void DomainProductPackageRegistrationTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("sunrise-test.com", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        Assert.IsTrue(packageGroup.Domain.DomainName.Length > 0);
        IDomainProductPackage normalRegistrationPackage;

        Assert.IsTrue(packageGroup.InPreRegPhase == false);
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out normalRegistrationPackage));

        Assert.IsTrue(normalRegistrationPackage.CurrentPrice.Price > 0);
        Assert.IsTrue(normalRegistrationPackage.ListPrice.Price > 0);

        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.ProductId > 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.CurrentPrice.Price > 0);
        Assert.IsTrue(string.IsNullOrEmpty(normalRegistrationPackage.DomainProductPackageItem.CustomXml));
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.Duration > 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.ListPrice.Price > 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.Name.Length > 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.ProductId > 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.Quantity == 1);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.ChildItems.Count == 0);
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.BasketAttributes.Count > 0);
      }
    }

    [TestMethod]
    public void DomainProductPackageItemExistTest()
    {
      var domains = DomainSearch.SearchDomain("spoonymac-rastaman.com", SOURCE_CODE, string.Empty);

      var domainProductPackages = DomainProductPackageProvider.BuildDomainProductPackageGroups(domains.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));
    }

    [TestMethod]
    public void DomainTrusteeRegistrationProductIdTest()
    {
        var domainSearchResponse = DomainSearch.SearchDomain("ihopethisdomaindoesnotexist.SG", SOURCE_CODE, string.Empty);

        var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

        Assert.IsTrue(packageGroups.Count() > 0);

        foreach (var packageGroup in packageGroups)
        {
            IDomainProductPackage domainProdPackage;
            packageGroup.TryGetRegistrationPackage(out domainProdPackage);

            IProductPackageItem trusteePrdPackage;
            Assert.IsTrue(domainProdPackage.TryGetTrusteePackage(out trusteePrdPackage));

            Assert.IsTrue(trusteePrdPackage.ProductId != 0);
            Assert.IsTrue(trusteePrdPackage.ProductId == 46307);
        }
    }

    [TestMethod]
    public void DomainProductPackageNullTest()
    {
      var domains = DomainSearch.SearchDomain("google.com", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domains.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        Assert.IsTrue(packageGroup.InPreRegPhase == false);
        Assert.IsTrue(packageGroup.PreRegPhasePackages.Count == 0);

        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage));
      }
    }

    [TestMethod]
    public void DomainProductPackageRegistrationSerializeTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("sunrise-test.com", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      StoreProvider.SaveDomainProductPackages(packageGroups);

      packageGroups = null;

      Assert.IsTrue(StoreProvider.TryGetDomainProductPackages(out packageGroups));

      var domainRegistrationProductPackageGroups = packageGroups as IDomainRegistrationProductPackageGroup[] ?? packageGroups.ToArray();

      Assert.IsTrue(domainRegistrationProductPackageGroups.Any());

      foreach (var packageGroup in domainRegistrationProductPackageGroups)
      {
        IDomainProductPackage registrationPackage;

        Assert.IsFalse(packageGroup.InPreRegPhase);
        Assert.IsFalse(packageGroup.PreRegPhasePackages.Any());

        Assert.IsTrue(packageGroup.Domain.DomainName == "sunrise-test.com");
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage));


        Assert.IsTrue(registrationPackage.CurrentPrice.Price > 0);
        Assert.IsTrue(registrationPackage.ListPrice.Price > 0);


        Assert.IsTrue(registrationPackage.DomainProductPackageItem.ProductId > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.CurrentPrice.Price > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.Duration > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.ListPrice.Price > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.Name.Length > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.ProductId > 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.Quantity == 1);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.ChildItems.Count == 0);
        Assert.IsTrue(registrationPackage.DomainProductPackageItem.BasketAttributes.Count > 0);
      }
    }

    [TestMethod]
    public void DomainProductPackageGroupInPreRegTest()
    {
      var mockTuple = CreateStandardDependencyMockObjects();

      var packageLookUp = DomainProductPackageLookUp.Create(1, LaunchPhases.SunriseA, "o1.borg");
      mockTuple.Item2.Setup(d => d.GetDotTypeInfo("Test").MinRegistrationLength).Returns(1);
      mockTuple.Item2.Setup(d => d.GetDotTypeInfo("Test").IsPreRegPhaseActive).Returns(true);

      var unitUnderTest = new DomainProductPackageProvider(mockTuple.Item1.Object as IProviderContainer);
      var packageGroup = unitUnderTest.BuildDomainProductPackageGroup(packageLookUp);

      Assert.IsTrue(packageGroup.InPreRegPhase);
      Assert.IsTrue(packageGroup.PreRegPhasePackages.Any());
      //Don't want to test the DotType provider - the prices need to be mocked out so that just the code in this provider is tested
      Assert.IsTrue(packageGroup.PreRegPhasePackages.ToList()[0].Value.CurrentPrice.Price > 0);
      Assert.IsTrue(packageGroup.PreRegPhasePackages.ToList()[0].Value.ListPrice.Price > 0);
    }

    [TestMethod]
    public void DomainProductPackageGroupRegistrationTest()
    {
      var packageLookUp = DomainProductPackageLookUp.Create(1, LaunchPhases.GeneralAvailability, "com");

      var packageGroup = DomainProductPackageProvider.BuildDomainProductPackageGroup(packageLookUp);

      IDomainProductPackage productPackage;

      Assert.IsFalse(packageGroup.InPreRegPhase);
      Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out productPackage));
      Assert.IsTrue(productPackage.CurrentPrice.Price > 0);
      Assert.IsTrue(productPackage.ListPrice.Price > 0);
    }

    [TestMethod]
    public void DomainProductPackageSerializeExceptionTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("blue-spoon-test123.com", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));
      
      Assert.IsFalse(StoreProvider.TryGetDomainProductPackages(out packageGroups));
    }
    
    [TestMethod]
    public void DomainProductPackageGuruTierIdTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("ant.london", SOURCE_CODE, string.Empty, new List<string> { "guru" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      StoreProvider.SaveDomainProductPackages(packageGroups);

      packageGroups = null;
      Assert.IsTrue(StoreProvider.TryGetDomainProductPackages(out packageGroups));


      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage));
        Assert.IsTrue(registrationPackage.TierId == 6);
      }
    }

    private Tuple<Mock<IProviderContainer>, Mock<IDotTypeProvider>, Mock<IDotTypeInfo>, Mock<ICurrencyProvider>> CreateStandardDependencyMockObjects()
    {
      Mock<IProviderContainer> mockContainer = new Mock<IProviderContainer>();
      Mock<IDotTypeProvider> mockDotTypeProvider = new Mock<IDotTypeProvider>();
      Mock<IDotTypeInfo> mockDotTypeInfo = new Mock<IDotTypeInfo>();
      Mock<ICurrencyProvider> mockCurrencyProvider = new Mock<ICurrencyProvider>();
      mockContainer.Setup(m => m.Resolve<IDotTypeProvider>()).Returns(mockDotTypeProvider.Object);

      var returnTuple = new Tuple<Mock<IProviderContainer>, Mock<IDotTypeProvider>, Mock<IDotTypeInfo>, Mock<ICurrencyProvider>>
        (
        mockContainer, mockDotTypeProvider, mockDotTypeInfo, mockCurrencyProvider
        );

      return returnTuple;
    }
  }
}
