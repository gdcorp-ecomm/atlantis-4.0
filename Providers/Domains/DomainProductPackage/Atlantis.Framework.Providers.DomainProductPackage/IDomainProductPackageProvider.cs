using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public interface IDomainProductPackageProvider
  {
    IEnumerable<IDomainRegistrationProductPackageGroup> BuildDomainProductPackageGroups(IList<IFindResponseDomain> findResponseDomains);
  }
}
