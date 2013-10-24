﻿using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
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
    public void DomainProductPackagePreRegTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("sunrise-test.o2.borg", SOURCE_CODE, string.Empty, new List<string> { "o2.borg" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        Assert.IsTrue(packageGroup.InLaunchPhase);
        Assert.IsTrue(packageGroup.Domain.DomainName.Length > 0);
        Assert.IsTrue(packageGroup.LaunchPhasePackages.Count > 0);
        Assert.IsTrue(packageGroup.TierId.HasValue);

        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage) == false);
      }
    }

    [TestMethod]  
    public void DomainProductPackagePreRegPriceTest()
    {
      var domainSearchResponse = DomainSearch.SearchDomain("sunrise-test.o2.borg", SOURCE_CODE, string.Empty, new List<string> { "o2.borg" });

      var packageGroups = DomainProductPackageProvider.BuildDomainProductPackageGroups(domainSearchResponse.GetDomainsByGroup(DomainGroupTypes.EXACT_MATCH));

      Assert.IsTrue(packageGroups.Count() > 0);

      foreach (var packageGroup in packageGroups)
      {
        var domainProductPackage = packageGroup.LaunchPhasePackages.ToList()[0];

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
        
        Assert.IsTrue(packageGroup.InLaunchPhase == false);
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
        Assert.IsTrue(packageGroup.InLaunchPhase == false);
        Assert.IsTrue(packageGroup.LaunchPhasePackages.Count == 0);
        
        IDomainProductPackage registrationPackage;
        Assert.IsTrue(packageGroup.TryGetRegistrationPackage(out registrationPackage));
      }
    }
  }
}