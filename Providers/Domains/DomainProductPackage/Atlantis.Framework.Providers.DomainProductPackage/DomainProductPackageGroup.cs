using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainRegistrationProductPackageGroup  : IDomainRegistrationProductPackageGroup
  {
    public IDomainProductPackage RegistrationPackage { get; set; }
    
    public IDomain Domain { get; set; }

    public bool InPreRegPhase { get; set; }

    private IDictionary<LaunchPhases, IDomainProductPackage> _preRegPhasePackages;
    public IDictionary<LaunchPhases, IDomainProductPackage> PreRegPhasePackages {
      get
      {
        return _preRegPhasePackages ?? (_preRegPhasePackages = new Dictionary<LaunchPhases, IDomainProductPackage>(0));
      }
    }

    public bool TryGetRegistrationPackage(out IDomainProductPackage registrationPackage)
    {
      registrationPackage = RegistrationPackage;
      
      return registrationPackage != null;
    }
  }
}