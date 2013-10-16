using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Domains.Interface
{
  public class DomainRegistrationProductPackageGroup  : IDomainRegistrationProductPackageGroup
  {
    public int? TierId { get; set; }

    public IDomain Domain { get; set; }

    public bool InPreRegPhase { get; set; }

    public IDictionary<LaunchPhases, IDomainProductPackage> PreRegPackages { get; set; }

    public IDomainProductPackage RegistrationPackage { get; set; }
  }
}