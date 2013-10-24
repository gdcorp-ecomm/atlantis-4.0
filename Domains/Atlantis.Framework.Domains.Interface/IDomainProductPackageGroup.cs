using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Domains.Interface
{
  public interface IDomainRegistrationProductPackageGroup 
  {
    int? TierId { get; set; }
    
    IDomain Domain { get; }

    bool InLaunchPhase { get; }

    IDictionary<LaunchPhases, IDomainProductPackage> LaunchPhasePackages { get; }

    bool TryGetRegistrationPackage(out IDomainProductPackage registrationPackage);
  }
}