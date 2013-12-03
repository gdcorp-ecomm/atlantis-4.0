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

    // This test may fail depending on the tld and what phase it is in for this date.
    [TestMethod]
    public void DomainProductPackagePreRegTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("pre-reg-test.menu", SOURCE_CODE, string.Empty, new List<string> {"menu"});

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        Assert.IsTrue(packageGroup.InPreRegPhase);
        Assert.IsTrue(packageGroup.Domain.DomainName.Length > 0);

        IDomainProductPackage launchphasePackage;
        Assert.IsTrue(packageGroup.PreRegPhasePackages.TryGetValue(LaunchPhases.Landrush, out launchphasePackage));
        Assert.IsTrue(launchphasePackage.TierId == 18);

        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage) == false);
      }
    }

    [TestMethod]
    public void DomainProductPackagePreRegPriceTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("sunrise-test.menu", SOURCE_CODE, string.Empty, new List<string> {"menu"});

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        var domainProductPackage = packageGroup.PreRegPhasePackages.ToList()[0];

        Assert.IsTrue(domainProductPackage.Value.CurrentPrice.Price > 0);
        Assert.IsTrue(domainProductPackage.Value.ListPrice.Price > 0);

        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage) == false);
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
        Assert.IsTrue(normalRegistrationPackage.DomainProductPackageItem.BasketAttributes.Count == 0);
      }
    }

    [TestMethod]
    public void DomainProductPackageItemExistTest()
    {
      var domains = DomainSearch.SearchDomain("spoonymac-rastaman.com", SOURCE_CODE, string.Empty);

      var domainProductPackages = DomainProductPackageProvider.BuildDomainProductPackageGroups(domains.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      //var domainItem = domainProductPackages.FirstOrDefault(dpp => dpp.DomainProductPackageItem != null).DomainProductPackageItem;

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
    public void DomainProductPackageApplicationFeeTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("appfee123-test.menu", SOURCE_CODE, string.Empty, new List<string> { "menu" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        var landRushProductPackage = packageGroup.PreRegPhasePackages.ToList().FirstOrDefault(pkg => pkg.Key == LaunchPhases.Landrush);

        ICurrencyPrice currentPrice;

        IProductPackageItem appFeePackage;
        Assert.IsTrue(landRushProductPackage.Value.TryGetApplicationFeePackage(out appFeePackage));

        Assert.IsTrue(appFeePackage.ProductId == 42753);

        Assert.IsTrue(landRushProductPackage.Value.TryGetApplicationFee(out currentPrice));
        Assert.IsTrue(currentPrice.Price == appFeePackage.CurrentPrice.Price);
      }
    }

    [TestMethod]  
    public void DomainProductPackageLaunchPhaseSerializeTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("blue-test.menu", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));
      
      StoreProvider.SaveDomainProductPackages(packageGroups);

      Assert.IsTrue(StoreProvider.TryGetDomainProductPackages(out packageGroups));

      Assert.IsTrue(packageGroups.Any());

      foreach (var packageGroup in packageGroups)
      {
        IDomainProductPackage registrationPackage;

        Assert.IsTrue(packageGroup.InPreRegPhase);
        Assert.IsTrue(packageGroup.Domain.DomainName == "blue-test.menu");
        Assert.IsTrue(packageGroup.PreRegPhasePackages.Count > 0);
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage) == false);
      
        var domainProductPackage = packageGroup.PreRegPhasePackages.ToList()[0];

        Assert.IsTrue(domainProductPackage.Value.CurrentPrice.Price > 0);
        Assert.IsTrue(domainProductPackage.Value.ListPrice.Price > 0);

        IDomainProductPackage launchphasePackage;

        if (packageGroup.PreRegPhasePackages.TryGetValue(LaunchPhases.SunriseA, out launchphasePackage))
        {
          Assert.IsTrue(launchphasePackage.TierId == 17);
        }

        Assert.IsFalse(packageGroup.TryGetRegistrationPackage(out registrationPackage));
      }
    }

    [TestMethod]
    public void DomainProductPackageRegistrationSerializeTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("blue-spoon-test123.com", SOURCE_CODE, string.Empty);

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      StoreProvider.SaveDomainProductPackages(packageGroups);

      Assert.IsTrue(StoreProvider.TryGetDomainProductPackages(out packageGroups));

      Assert.IsTrue(packageGroups.Any());

      foreach (var packageGroup in packageGroups)
      {
        IDomainProductPackage registrationPackage;

        Assert.IsFalse(packageGroup.InPreRegPhase);
        Assert.IsFalse(packageGroup.PreRegPhasePackages.Any());

        Assert.IsTrue(packageGroup.Domain.DomainName == "blue-spoon-test123.com");
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage));


        Assert.IsTrue(registrationPackage.CurrentPrice.Price > 0);
        Assert.IsTrue(registrationPackage.ListPrice.Price > 0);
      }
    }

    [TestMethod]
    public void DomainProductPackageGroupInPreRegTest()
    {
      var packageLookUp = DomainProductPackageLookUp.Create(1, LaunchPhases.SunriseA, "o1.borg");

      var packageGroup = DomainProductPackageProvider.BuildDomainProductPackageGroup(packageLookUp);

      Assert.IsTrue(packageGroup.InPreRegPhase);
      Assert.IsTrue(packageGroup.PreRegPhasePackages.Any());
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
    public void DomainProductPackageESTATEPreRegTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("pre-reg-test.ESTATE", SOURCE_CODE, string.Empty, new List<string> { "ESTATE" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      var package = packageGroups.ToList()[0];

      Assert.IsTrue(package.InPreRegPhase);
      Assert.IsTrue(package.Domain.DomainName.Length > 0);

      IDomainProductPackage launchphasePackage;
      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration1Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 51193);
      Assert.IsTrue(launchphasePackage.TierId == 10);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration2Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 51113);
      Assert.IsTrue(launchphasePackage.TierId == 10);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration3Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 51027);
      Assert.IsTrue(launchphasePackage.TierId == 10);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration4Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 50993);
      Assert.IsTrue(launchphasePackage.TierId == 10);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration5Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 50973);
      Assert.IsTrue(launchphasePackage.TierId == 10);

      IDomainProductPackage registrationPackage;
      Assert.IsTrue(package.TryGetRegistrationPackage(out registrationPackage) == false);
    }

    [TestMethod]
    public void DomainProductPackagePHOTOGRAPHYPreRegTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("pre-reg-test.PHOTOGRAPHY", SOURCE_CODE, string.Empty, new List<string> { "PHOTOGRAPHY" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      var package = packageGroups.ToList()[0];

      Assert.IsTrue(package.InPreRegPhase);
      Assert.IsTrue(package.Domain.DomainName.Length > 0);

      IDomainProductPackage launchphasePackage;
      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration1Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 52855);
      Assert.IsTrue(launchphasePackage.TierId == 11);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration2Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 52815);
      Assert.IsTrue(launchphasePackage.TierId == 11);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration3Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 52775);
      Assert.IsTrue(launchphasePackage.TierId == 11);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration4Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 52735);
      Assert.IsTrue(launchphasePackage.TierId == 11);

      Assert.IsTrue(package.PreRegPhasePackages.TryGetValue(LaunchPhases.EarlyRegistration5Day, out launchphasePackage));
      Assert.IsTrue(launchphasePackage.DomainProductPackageItem.ProductId == 52695);
      Assert.IsTrue(launchphasePackage.TierId == 11);

      IDomainProductPackage registrationPackage;
      Assert.IsTrue(package.TryGetRegistrationPackage(out registrationPackage) == false);
    }

    //[TestMethod]
    //public void SetRegistrationLengthGeneralAvailTest()
    //{
    //  var domainProductPackage = new DomainProductPackage(ProviderContainer);
    //  domainProductPackage.Domain = new Domain("test-reglength", "com");

    //  var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, 101, 1, 1, ProviderContainer);
    //  domainProductPackage.PackageItems.Add(productPackageItem);

    //  Assert.IsFalse(domainProductPackage.TrySetRegistrationLength(11, 1, LaunchPhases.Landrush));
    //  Assert.IsTrue(domainProductPackage.TrySetRegistrationLength(6, 1, LaunchPhases.GeneralAvailability));

    //  Assert.IsTrue(domainProductPackage.DomainProductPackageItem.ProductId == 106);
    //}

    //[TestMethod]
    //public void SetRegistrationLengthPreRegTest()
    //{
    //  var domainProductPackage = new DomainProductPackage(ProviderContainer);
    //  domainProductPackage.Domain = new Domain("test-reglength", "menu");

    //  var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, 42128, 1, 1, ProviderContainer);
    //  domainProductPackage.PackageItems.Add(productPackageItem);

    //  Assert.IsTrue(domainProductPackage.TrySetRegistrationLength(1, 1, LaunchPhases.Landrush));

    //  Assert.IsTrue(domainProductPackage.DomainProductPackageItem.ProductId == 42774);
    //}
  }
}
