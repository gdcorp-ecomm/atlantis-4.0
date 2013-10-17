using System;
using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackageProvider : ProviderBase, IDomainProductPackageProvider
  {
    private readonly Lazy<IDotTypeProvider> _dotTypeProvider;
    
    public DomainProductPackageProvider(IProviderContainer container) : base(container)
    {
      _dotTypeProvider = new Lazy<IDotTypeProvider>(() => Container.Resolve<IDotTypeProvider>());
    }

    private DomainRegistrationProductPackageGroup GetDomainRegistrationProductPackageGroup(IFindResponseDomain findResponseDomain, int domainCount)
    {
      var dotTypeInfo = _dotTypeProvider.Value.GetDotTypeInfo(findResponseDomain.Domain.Tld);

      var domainProductPackageGroup = new DomainRegistrationProductPackageGroup
      {
        Domain = findResponseDomain.Domain,
        InLaunchPhase = findResponseDomain.InPreRegPhase,
        TierId = findResponseDomain.InternalTier
      };

      var years = dotTypeInfo.MinRegistrationLength;
      var tierId = findResponseDomain.InternalTier;

      if (domainProductPackageGroup.InLaunchPhase)
      {
        foreach (var launchPhase in findResponseDomain.PreRegLaunchPhases)
        {
          var domainProductLookup = DomainProductLookup.Create(years, domainCount, launchPhase, TLDProductTypes.Registration, tierId);
          var productid = dotTypeInfo.GetProductId(domainProductLookup);

          var domainProductPackage = new DomainProductPackage(Container) { Domain = findResponseDomain.Domain, TierId = findResponseDomain.InternalTier };

          var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
          domainProductPackage.PackageItems.Add(productPackageItem);

          domainProductPackageGroup.LaunchPhasePackages.Add(launchPhase, domainProductPackage);
        }
      }
      else
      {
        const LaunchPhases launchPhase = LaunchPhases.GeneralAvailability;
        var domainProductLookup = DomainProductLookup.Create(years, domainCount, launchPhase, TLDProductTypes.Registration, tierId);
        var productid = dotTypeInfo.GetProductId(domainProductLookup);

        var domainProductPackage = new DomainProductPackage(Container) { Domain = findResponseDomain.Domain, TierId = findResponseDomain.InternalTier };

        var productPackageItem = ProductPackageItem.Create(DomainProductPackage.PACKAGE_NAME, productid, 1, years, Container);
        domainProductPackage.PackageItems.Add(productPackageItem);

        domainProductPackageGroup.RegistrationPackage = domainProductPackage;
      }
      return domainProductPackageGroup;
    }

    public IEnumerable<IDomainRegistrationProductPackageGroup> BuildDomainProductPackageGroups(IList<IFindResponseDomain> findResponseDomains)
    {
      // TODO: Add pre-reg app fee if it exists (use to display on the list page)
      // TODO: Total currency for all products?

      var domainProductPackageGroups = new List<IDomainRegistrationProductPackageGroup>(findResponseDomains.Count);
      var domainCount = findResponseDomains.Count;

      foreach (var findResponseDomain in findResponseDomains)
      {
        var domainProductPackageGroup = GetDomainRegistrationProductPackageGroup(findResponseDomain, domainCount);

        domainProductPackageGroups.Add(domainProductPackageGroup);
      }

      return domainProductPackageGroups;
    }
  }
}
