﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackageProvider : ProviderBase, IDomainProductPackageProvider
  {
    private readonly Lazy<IDotTypeProvider> _dotTypeProvider;
    
    public DomainProductPackageProvider(IProviderContainer container) : base(container)
    {
      _dotTypeProvider = new Lazy<IDotTypeProvider>(() => Container.Resolve<IDotTypeProvider>());
    }

    private void SetApplicationFee(LaunchPhases launchPhase, IDomainProductPackage domainProductPackage, IDotTypeInfo dotTypeInfo)
    {
      var productIds = dotTypeInfo.GetPhaseApplicationProductIdList(launchPhase);
     
      if (productIds != null && productIds.Count > 0)
      {
        IProductPackageItem appFeeItem = ProductPackageItem.Create(DomainProductPackage.APPLICATION_FEE_NAME, productIds[0], 1, 1, Container);

        domainProductPackage.PackageItems.Add(appFeeItem);
      }
    }

    private DomainRegistrationProductPackageGroup GetDomainRegistrationProductPackageGroup(IFindResponseDomain findResponseDomain, int domainCount)
    {
      var dotTypeInfo = _dotTypeProvider.Value.GetDotTypeInfo(findResponseDomain.Domain.Tld);

      var domainRegProductPackageGroup = new DomainRegistrationProductPackageGroup
      {
        Domain = findResponseDomain.Domain,
        InPreRegPhase = findResponseDomain.InPreRegPhase,
      };

      var years = dotTypeInfo.MinRegistrationLength;

      if (domainRegProductPackageGroup.InPreRegPhase)
      {
        foreach (var launchPhaseItem in findResponseDomain.LaunchPhaseItems)
        {
          var domainProductLookup = DomainProductLookup.Create(years, domainCount, launchPhaseItem.LaunchPhase, TLDProductTypes.Registration, launchPhaseItem.TierId);
          var productid = dotTypeInfo.GetProductId(domainProductLookup);

          var domainProductPackage = new DomainProductPackage(Container) { Domain = findResponseDomain.Domain, TierId = launchPhaseItem.TierId};

          var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
          productPackageItem.BasketAttributes = findResponseDomain.CartAttributes;

          domainProductPackage.PackageItems.Add(productPackageItem);

          SetApplicationFee(launchPhaseItem.LaunchPhase, domainProductPackage, dotTypeInfo);

          domainRegProductPackageGroup.PreRegPhasePackages.Add(launchPhaseItem.LaunchPhase, domainProductPackage);
        }
      }
      else
      {
        const LaunchPhases launchPhase = LaunchPhases.GeneralAvailability;
        var domainProductLookup = DomainProductLookup.Create(years, domainCount, launchPhase, TLDProductTypes.Registration, findResponseDomain.InternalTier);
        var productid = dotTypeInfo.GetProductId(domainProductLookup);

        var domainProductPackage = new DomainProductPackage(Container) { Domain = findResponseDomain.Domain, TierId = findResponseDomain.InternalTier };

        var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
        productPackageItem.BasketAttributes = findResponseDomain.CartAttributes;

        domainProductPackage.PackageItems.Add(productPackageItem);

       domainRegProductPackageGroup.RegistrationPackage = domainProductPackage;
      }

        
      return domainRegProductPackageGroup;
    }

    public IEnumerable<IDomainRegistrationProductPackageGroup> BuildDomainProductPackageGroups(IList<IFindResponseDomain> findResponseDomains)
    {
      var domainProductPackageGroups = new List<IDomainRegistrationProductPackageGroup>(findResponseDomains.Count);

      // This value is used to determine the correct productId and price
      // If the value is > 1 then a bulk pricing product id will be returned as the logic will interpret that as buying multiple domains
      // This value should be 1 as the findResponse holds all the domains returned from the search. The customer is not purchasing all the returned domains.
      // A single price / product Id is required here.
      var domainCount = 1;

      foreach (var findResponseDomain in findResponseDomains)
      {
        var domainProductPackageGroup = GetDomainRegistrationProductPackageGroup(findResponseDomain, domainCount);

        domainProductPackageGroups.Add(domainProductPackageGroup);
      }
      
      return domainProductPackageGroups;
    }

    public IDomainRegistrationProductPackageGroup BuildDomainProductPackageGroup(DomainProductPackageLookUp packageLookUp)
    {
      var dotTypeInfo = _dotTypeProvider.Value.GetDotTypeInfo(packageLookUp.Tld);
      var years = dotTypeInfo.MinRegistrationLength;
      
      var domainProductPackageGroup = new DomainRegistrationProductPackageGroup
      {
        Domain = new Domain(packageLookUp.Sld, packageLookUp.Tld),
        InPreRegPhase = dotTypeInfo.IsPreRegPhaseActive
      };


      if (dotTypeInfo.IsPreRegPhaseActive)
      {
          var domainProductLookup = DomainProductLookup.Create(years, packageLookUp.DomainCount, packageLookUp.LaunchPhase, packageLookUp.ProductType);
          var productid = dotTypeInfo.GetProductId(domainProductLookup);

          var domainProductPackage = new DomainProductPackage(Container) { Domain = domainProductPackageGroup.Domain };

          var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
          domainProductPackage.PackageItems.Add(productPackageItem);

          SetApplicationFee(packageLookUp.LaunchPhase, domainProductPackage, dotTypeInfo);

          domainProductPackageGroup.PreRegPhasePackages.Add(packageLookUp.LaunchPhase, domainProductPackage);
      }
      else
      {
        var domainProductLookup = DomainProductLookup.Create(years, packageLookUp.DomainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration);
        var productid = dotTypeInfo.GetProductId(domainProductLookup);

        var domainProductPackage = new DomainProductPackage(Container) { Domain = domainProductPackageGroup.Domain };

        var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
        domainProductPackage.PackageItems.Add(productPackageItem);

        domainProductPackageGroup.RegistrationPackage = domainProductPackage;
      }
      
      return domainProductPackageGroup;
    }
  }
}
