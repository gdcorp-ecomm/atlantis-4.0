using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainRegistrationProductPackageGroup  : IDomainRegistrationProductPackageGroup
  {
    public IDomainProductPackage RegistrationPackage { get; set; }
    
    public IDomain Domain { get; set; }

    public bool InLaunchPhase { get; set; }

    private IDictionary<LaunchPhases, IDomainProductPackage> _launchPhasePackages;
    public IDictionary<LaunchPhases, IDomainProductPackage> LaunchPhasePackages {
      get
      {
        if (_launchPhasePackages == null)
        {
          _launchPhasePackages = new Dictionary<LaunchPhases, IDomainProductPackage>(0);
        }

        return _launchPhasePackages;
      }
    }

    public bool TryGetRegistrationPackage(out IDomainProductPackage registrationPackage)
    {
      registrationPackage = RegistrationPackage;
      
      return registrationPackage != null;
    }
  }
}