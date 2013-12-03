using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage.Interface
{
  public interface IDomainRegistrationProductPackageGroup 
  {
    IDomain Domain { get; }

    bool InPreRegPhase { get; }

    IDictionary<LaunchPhases, IDomainProductPackage> PreRegPhasePackages { get; }

    bool TryGetRegistrationPackage(out IDomainProductPackage registrationPackage);
  }
}