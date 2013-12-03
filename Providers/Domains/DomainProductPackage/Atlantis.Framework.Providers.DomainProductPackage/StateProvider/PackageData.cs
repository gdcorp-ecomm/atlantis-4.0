using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage.StateProvider
{
  [DataContract(Name = "package")]
  internal class PackageData 
  {
    [DataMember(Name="domnam")]
    public string PunyCodeDomainName { get; set; }
    [DataMember(Name = "sld")]
    public string Sld { get; set; }
    [DataMember(Name = "punysld")]
    public string PunyCodeSld { get; set; }
    [DataMember(Name = "tld")]
    public string Tld { get; set; }
    [DataMember(Name = "punytld")]
    public string PunyCodeTld { get; set; }

    [DataMember(Name = "tierid")]
    public int TierId { get; set; }

    [DataMember(Name = "inphz")]
    public bool InLaunchPhase { get; set; }

    [DataMember(Name = "phzpkg")]
    public Dictionary<LaunchPhases, List<IDictionary<string, object>>> LaunchPhasePackages;

    [DataMember(Name = "regpkg")] 
    public Dictionary<string, object> RegistrationPackage;


  }
}
