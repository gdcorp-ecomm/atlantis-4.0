using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public class DomainProductLookup : IDomainProductLookup
  {
    private DomainProductLookup(int years, int domainCount, LaunchPhases launchPhase, TLDProductTypes productType, int? priceTierId = null, int? registryId = null)
    {
      Years = years;
      DomainCount = domainCount;
      TldPhase = launchPhase;
      ProductType = productType;
      PriceTierId = priceTierId;
      RegistryId = registryId;
    }

    public static IDomainProductLookup Create(int years, int domainCount, LaunchPhases launchPhase, TLDProductTypes productType, int? priceTierId = null, int? registryId = null)
    {
      return new DomainProductLookup(years, domainCount, launchPhase, productType, priceTierId, registryId);
    }

    public int Years { get; private set; }

    public int DomainCount { get; private set; }

    public LaunchPhases TldPhase { get; private set; }

    public TLDProductTypes ProductType { get; private set; }

    public int? PriceTierId { get; private set; }

    public int? RegistryId { get; private set; }
  }
}
