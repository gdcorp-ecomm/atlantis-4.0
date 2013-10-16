using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Domains.Interface
{
  public interface IDomainRegistrationProductPackageGroup 
  {
    int? TierId { get; set; }
    
    IDomain Domain { get; }

    bool InPreRegPhase { get; }

    IDictionary<LaunchPhases, IDomainProductPackage> PreRegPackages { get; }

    IDomainProductPackage RegistrationPackage { get; }
  }
}