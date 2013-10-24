using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;


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

    public static IEnumerable<IDomainRegistrationProductPackageGroup> PackageDataToDomainRegistratoinProduct(IList<PackageData> packageDataItems, IProviderContainer container)
    {
      var domainRegProductGroupPackages = new List<IDomainRegistrationProductPackageGroup>(packageDataItems.Count);

      foreach (var packageDataItem in packageDataItems)
      {
        var domainRegProductPackageGroup = new DomainRegistrationProductPackageGroup();
        
        // Build the domain.
        domainRegProductPackageGroup.Domain = new Domain(packageDataItem.Sld, packageDataItem.Tld, packageDataItem.PunyCodeSld, packageDataItem.PunyCodeTld);
        domainRegProductPackageGroup.InLaunchPhase = packageDataItem.InLaunchPhase;
        domainRegProductPackageGroup.TierId = packageDataItem.TierId;

        if (packageDataItem.InLaunchPhase)
        {
          foreach (var key in packageDataItem.LaunchPhasePackages.Keys)
          {
            var launchPhaseDomainProductPackage = new DomainProductPackage.DomainProductPackage(container);
            launchPhaseDomainProductPackage.Domain = domainRegProductPackageGroup.Domain;

            var launchPhasePackageDataItem = packageDataItem.LaunchPhasePackages[key];

            double duration;
            double.TryParse(launchPhasePackageDataItem[DURATION].ToString(), out duration);

            int productId;
            int.TryParse(launchPhasePackageDataItem[PRODUCT_ID].ToString(), out productId);

            int quantity;
            int.TryParse(launchPhasePackageDataItem[QUANTITY].ToString(), out quantity);

            var name = launchPhasePackageDataItem[PACKAGE_NAME].ToString();

            launchPhaseDomainProductPackage.PackageItems.Add(ProductPackageItem.Create(name, productId, quantity, duration, container));

            launchPhaseDomainProductPackage.DomainProductPackageItem.BasketAttributes = launchPhasePackageDataItem[BASKET_ATTRIBUTES] as Dictionary<string, string>;

            domainRegProductPackageGroup.LaunchPhasePackages.Add(key, launchPhaseDomainProductPackage);
          }
        }
        else
        {
          var registrationPackage = new DomainProductPackage.DomainProductPackage(container);
          registrationPackage.Domain = domainRegProductPackageGroup.Domain;
          domainRegProductPackageGroup.RegistrationPackage = registrationPackage;

          double duration;
          double.TryParse(packageDataItem.RegistrationPackage[DURATION].ToString(), out duration);

          int productId;
          int.TryParse(packageDataItem.RegistrationPackage[PRODUCT_ID].ToString(), out productId);

          int quantity;
          int.TryParse(packageDataItem.RegistrationPackage[QUANTITY].ToString(), out quantity);

          var name = packageDataItem.RegistrationPackage[PACKAGE_NAME].ToString();

          domainRegProductPackageGroup.RegistrationPackage.PackageItems.Add(ProductPackageItem.Create(name, productId, quantity, duration, container));
        }

        domainRegProductGroupPackages.Add(domainRegProductPackageGroup);
      }

      return domainRegProductGroupPackages;
    }

    public static IEnumerable<PackageData> DomainRegistratoinProductToPackageData(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistrationProductPackageGroup)
    {
      var packageDataItems = new List<PackageData>(0);

      foreach (var domainGroup in domainRegistrationProductPackageGroup)
      {
        var packageData = new PackageData
          {
            TierId = domainGroup.TierId,
            InLaunchPhase = domainGroup.InLaunchPhase,
            Tld = domainGroup.Domain.Tld,
            Sld = domainGroup.Domain.Sld,
            PunyCodeTld = domainGroup.Domain.PunyCodeTld,
            PunyCodeSld = domainGroup.Domain.PunyCodeSld,
            PunyCodeDomainName = domainGroup.Domain.PunyCodeDomainName
          };

        packageData.InLaunchPhase = domainGroup.InLaunchPhase;

        if (domainGroup.InLaunchPhase)
        {
          packageData.LaunchPhasePackages = new Dictionary<LaunchPhases, Dictionary<string, object>>(domainGroup.LaunchPhasePackages.Count);
          foreach (var package in domainGroup.LaunchPhasePackages)
          {
            // Get the values that need to go into the packageDataValues
            var packageItem = package.Value.DomainProductPackageItem;
            var dataItem = new Dictionary<string, object>(8)
              {
                {BASKET_ATTRIBUTES, packageItem.BasketAttributes}, 
                {CUSTOM_XML, packageItem.CustomXml}, 
                {DURATION, packageItem.Duration}, 
                {PACKAGE_NAME, packageItem.Name}, 
                {PRODUCT_ID, packageItem.ProductId}, 
                {QUANTITY, packageItem.Quantity}
              };

            packageData.LaunchPhasePackages.Add(package.Key, dataItem);
          }
        }
        else
        {
          IDomainProductPackage productPackage;
          if (domainGroup.TryGetRegistrationPackage(out productPackage))
          {
            //packageData.RegistrationPackage = new KeyValuePair<string, Dictionary<string, object>>();

            var packageItem = productPackage.DomainProductPackageItem;
            var dataItem = new Dictionary<string, object>(8)
              {
                {BASKET_ATTRIBUTES, packageItem.BasketAttributes}, 
                {CUSTOM_XML, packageItem.CustomXml}, 
                {DURATION, packageItem.Duration}, 
                {PACKAGE_NAME, packageItem.Name}, 
                {PRODUCT_ID, packageItem.ProductId}, 
                {QUANTITY, packageItem.Quantity}
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
