using System;
using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;
using Atlantis.Framework.Providers.Interface.Products;


namespace Atlantis.Framework.Providers.DomainProductPackageStateProvider
{
  internal class PackageBuilder
  {
    private const string BASKET_ATTRIBUTES = "basket";
    private const string CUSTOM_XML = "xml";
    private const string DURATION = "dur";
    private const string PACKAGE_NAME = "nam";
    private const string PRODUCT_ID = "pfid";
    private const string QUANTITY = "qty";
    private const string TIER_ID = "tierid";

    private static IProductPackageItem CreateProductPackageItem(IProviderContainer container, IDictionary<string, object> packageDataItem)
    {
      double duration;
      double.TryParse(packageDataItem[DURATION].ToString(), out duration);

      int productId;
      int.TryParse(packageDataItem[PRODUCT_ID].ToString(), out productId);

      int quantity;
      int.TryParse(packageDataItem[QUANTITY].ToString(), out quantity);

      var name = packageDataItem[PACKAGE_NAME].ToString();
      
      var productPackageItem = ProductPackageItem.Create(name, productId, quantity, duration, container);
      productPackageItem.BasketAttributes = packageDataItem[BASKET_ATTRIBUTES] as Dictionary<string, string>;

      return productPackageItem;
    }

    public static IEnumerable<IDomainRegistrationProductPackageGroup> PackageDataToDomainRegistrationProduct(IList<PackageData> packageDataItemGroups, IProviderContainer container)
    {
      var domainRegProductGroupPackages = new List<IDomainRegistrationProductPackageGroup>(packageDataItemGroups.Count);

      foreach (var packageDataItemGroup in packageDataItemGroups)
      {
        var domainRegProductPackageGroup = new DomainRegistrationProductPackageGroup();

        // Build the domain.
        domainRegProductPackageGroup.Domain = new Domain(packageDataItemGroup.Sld, packageDataItemGroup.Tld, packageDataItemGroup.PunyCodeSld, packageDataItemGroup.PunyCodeTld);
        domainRegProductPackageGroup.InLaunchPhase = packageDataItemGroup.InLaunchPhase;

        if (packageDataItemGroup.InLaunchPhase)
        {
          foreach (var key in packageDataItemGroup.LaunchPhasePackages.Keys)
          {
            var launchPhaseDomainProductPackage = new DomainProductPackage.DomainProductPackage(container);
            launchPhaseDomainProductPackage.Domain = domainRegProductPackageGroup.Domain;

            var launchPhasePackageDataItem = packageDataItemGroup.LaunchPhasePackages[key];

            foreach (var packageDataItemValues in launchPhasePackageDataItem)
            {
              launchPhaseDomainProductPackage.PackageItems.Add(CreateProductPackageItem(container, packageDataItemValues));
              
              int tierValue;
              object tierObj;
              if (packageDataItemValues.TryGetValue(TIER_ID, out tierObj) && int.TryParse(Convert.ToString(tierObj), out tierValue))
              {
                launchPhaseDomainProductPackage.TierId = tierValue;
              }
            }

            domainRegProductPackageGroup.LaunchPhasePackages.Add(key, launchPhaseDomainProductPackage);
          }
        }
        else
        {
          var registrationPackage = new DomainProductPackage.DomainProductPackage(container);
          registrationPackage.Domain = domainRegProductPackageGroup.Domain;
          domainRegProductPackageGroup.RegistrationPackage = registrationPackage;

          var productPackageItem = CreateProductPackageItem(container, packageDataItemGroup.RegistrationPackage);

          int tierValue;
          if (packageDataItemGroup.RegistrationPackage[TIER_ID] != null && int.TryParse(packageDataItemGroup.RegistrationPackage[TIER_ID].ToString(), out tierValue))
          {
            domainRegProductPackageGroup.RegistrationPackage.TierId = tierValue;
          }

          domainRegProductPackageGroup.RegistrationPackage.PackageItems.Add(productPackageItem);
        }

        domainRegProductGroupPackages.Add(domainRegProductPackageGroup);
      }

      return domainRegProductGroupPackages;
    }

    public static IEnumerable<PackageData> DomainRegistrationProductToPackageData(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistrationProductPackageGroup)
    {
      var packageDataItems = new List<PackageData>(0);

      foreach (var domainGroup in domainRegistrationProductPackageGroup)
      {
        var packageData = new PackageData
        {
          InLaunchPhase = domainGroup.InLaunchPhase,
          Tld = domainGroup.Domain.Tld,
          Sld = domainGroup.Domain.Sld,
          PunyCodeTld = domainGroup.Domain.PunyCodeTld,
          PunyCodeSld = domainGroup.Domain.PunyCodeSld,
          PunyCodeDomainName = domainGroup.Domain.PunyCodeDomainName,
        };

        packageData.InLaunchPhase = domainGroup.InLaunchPhase;

        if (domainGroup.InLaunchPhase)
        {
          packageData.LaunchPhasePackages = new Dictionary<LaunchPhases, List<IDictionary<string, object>>>(domainGroup.LaunchPhasePackages.Count);

          foreach (var launchPhasePackage in domainGroup.LaunchPhasePackages)
          {
            var launchPhaseDataItems = new List<IDictionary<string, object>>(launchPhasePackage.Value.PackageItems.Count);
            foreach (var packageItem in launchPhasePackage.Value.PackageItems)
            {
              var packageDataItem = new Dictionary<string, object>(8)
              {
                {BASKET_ATTRIBUTES, packageItem.BasketAttributes},
                {CUSTOM_XML, packageItem.CustomXml},
                {DURATION, packageItem.Duration},
                {PACKAGE_NAME, packageItem.Name},
                {PRODUCT_ID, packageItem.ProductId},
                {QUANTITY, packageItem.Quantity},
              };

              if (launchPhasePackage.Value.TierId != null)
              {
                packageDataItem.Add(TIER_ID, launchPhasePackage.Value.TierId.Value);
              }

              launchPhaseDataItems.Add(packageDataItem);
            }

            packageData.LaunchPhasePackages.Add(launchPhasePackage.Key, launchPhaseDataItems);
          }
        }
        else
        {
          IDomainProductPackage productPackage;
          if (domainGroup.TryGetRegistrationPackage(out productPackage))
          {
            var productPackageItem = productPackage.DomainProductPackageItem;
            var dataItem = new Dictionary<string, object>(8)
            {
              {BASKET_ATTRIBUTES, productPackageItem.BasketAttributes},
              {CUSTOM_XML, productPackageItem.CustomXml},
              {DURATION, productPackageItem.Duration},
              {PACKAGE_NAME, productPackageItem.Name},
              {PRODUCT_ID, productPackageItem.ProductId},
              {QUANTITY, productPackageItem.Quantity},
              {TIER_ID, packageData.TierId}
            };

            packageData.RegistrationPackage = dataItem;
          }
        }

        packageDataItems.Add(packageData);
      }

      return packageDataItems;
    }
  }
}
