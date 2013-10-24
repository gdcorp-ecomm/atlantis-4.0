using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackageStateProvider
{
  public interface IPersistanceStoreProvider
  {
    bool TryGetDomainProductPackages(out IEnumerable<IDomainRegistrationProductPackageGroup> domainProductPackages);

    void SaveDomainProductPackages(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistratoinProductPackageGroup);
  }
}
