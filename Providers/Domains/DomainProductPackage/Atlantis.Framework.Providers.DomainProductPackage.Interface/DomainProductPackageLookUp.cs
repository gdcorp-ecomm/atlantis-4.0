using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Providers.DomainProductPackage.Interface
{
  public class DomainProductPackageLookUp
  {
    public int DomainCount { get; set; }
    public LaunchPhases LaunchPhase { get; set; }
    public string Tld { get; set; }
    public string Sld { get; set; }
    public TLDProductTypes ProductType { get; set; }

    public static DomainProductPackageLookUp Create(int domainCount, LaunchPhases launchPhase, string tld, TLDProductTypes productType = TLDProductTypes.Registration)
    {
      var lookUp = new DomainProductPackageLookUp
        {
          DomainCount = domainCount, 
          LaunchPhase = launchPhase,
          Tld = tld,
          ProductType = productType
        };

      return lookUp;
    }
  }
}
