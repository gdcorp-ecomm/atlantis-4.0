using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackageLookUp
  {
    internal int DomainCount { get; set; }
    internal LaunchPhases LaunchPhase { get; set; }
    internal string Tld { get; set; }
    internal string Sld { get; set; }
    internal TLDProductTypes ProductType { get; set; }

    public static DomainProductPackageLookUp Create(int domainCount, LaunchPhases launchPhase, string sld, string tld, TLDProductTypes productType = TLDProductTypes.Registration)
    {
      var lookUp = new DomainProductPackageLookUp
        {
          DomainCount = domainCount, 
          LaunchPhase = launchPhase, 
          Sld = sld,
          Tld = tld,
          ProductType = productType
        };

      return lookUp;
    }
  }
}
