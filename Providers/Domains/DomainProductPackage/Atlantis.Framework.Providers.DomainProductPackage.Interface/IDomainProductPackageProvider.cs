using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage.Interface
{
  public interface IDomainProductPackageProvider
  {
    IDomainRegistrationProductPackageGroup BuildDomainProductPackageGroup(DomainProductPackageLookUp packageLookUp);
    IEnumerable<IDomainRegistrationProductPackageGroup> BuildDomainProductPackageGroups(IList<IFindResponseDomain> findResponseDomains);
  }
}
