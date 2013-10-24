using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.DomainProductPackageStateProvider
{
  public class PersistanceStoreProvider : ProviderBase, IPersistanceStoreProvider
  {
    private const string SESSION_PACKAGE_ID = "DomainProductPackageSerializer.DomainProductPackage";

    public PersistanceStoreProvider(IProviderContainer container) : base(container){}

    public static IPersistanceStore SessionState = new PersistanceStore();

    public bool TryGetDomainProductPackages(out IEnumerable<IDomainRegistrationProductPackageGroup> domainProductPackages)
    {
      bool success;
      domainProductPackages = new List<IDomainRegistrationProductPackageGroup>(0);
      var packageDataSerializedString = string.Empty;

      try
      {
        packageDataSerializedString = SessionState.Get(SESSION_PACKAGE_ID);

        IEnumerable<PackageData> packageDataItems;
        if (Serializer.TryGetPackageDataItems(packageDataSerializedString, out packageDataItems))
        {
          domainProductPackages = PackageBuilder.PackageDataToDomainRegistratoinProduct(packageDataItems.ToList(), Container);
        }

        success = domainProductPackages.Any();
      }
      catch (Exception ex)
      {
        success = false;
        Engine.Engine.LogAtlantisException(new AtlantisException("DomainProductPackageStateProvider.TryRebuildDomainProductPackages", 0, ex.ToString(), packageDataSerializedString));
      }

      return success;
    }

    public void SaveDomainProductPackages(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistratoinProductPackageGroup)
    {
      var packageDataItems = PackageBuilder.DomainRegistratoinProductToPackageData(domainRegistratoinProductPackageGroup);

      var output = JsonConvert.SerializeObject(packageDataItems);

      SessionState.Save(SESSION_PACKAGE_ID, output);
    }
  }
}
