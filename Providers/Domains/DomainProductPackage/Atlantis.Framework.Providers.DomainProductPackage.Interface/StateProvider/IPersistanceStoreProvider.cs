using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainProductPackage.Interface.StateProvider
{
  public interface IPersistanceStoreProvider
  {
    bool TryGetDomainProductPackages(out IEnumerable<IDomainRegistrationProductPackageGroup> domainProductPackages);

    void SaveDomainProductPackages(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistratoinProductPackageGroup);
  }
}
