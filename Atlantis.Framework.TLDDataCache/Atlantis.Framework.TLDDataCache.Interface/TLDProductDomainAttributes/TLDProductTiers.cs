using System.Collections.Generic;

namespace Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes
{
  public class TLDProductTiers
  {
    private readonly SortedList<int, TLDProductTier> _tierGroups;

    public int PriceTierId { get; set; }
    public int RegistryId { get; set; }

    internal TLDProductTiers()
    {
      _tierGroups = new SortedList<int, TLDProductTier>(5);
    }

    public bool TryGetProduct(int years, int domainCount, out TLDProduct product)
    {
      product = null;
      bool result = false;

      TLDProductTier tier = GetValidTier(domainCount);
      if (tier != null)
      {
        result = tier.TryGetProduct(years, out product);
      }

      return result;
    }

    public bool ValidTierExist(int domainCount)
    {
      bool result = false;

      foreach (var tierPair in _tierGroups)
      {
        if ((domainCount >= tierPair.Key))
        {
          result = true;
          break;
        }
      }

      return result;
    }

    private TLDProductTier GetValidTier(int domainCount)
    {
      TLDProductTier result = null;

      foreach (var tierPair in _tierGroups)
      {
        if ((domainCount >= tierPair.Key))
        {
          result = tierPair.Value;
        }
      }

      return result;
    }

    internal void AddDotTypeTier(TLDProductTier dotTypeTier)
    {
      _tierGroups.Add(dotTypeTier.MinDomainCount, dotTypeTier);
    }

    internal bool TryGetTier(int exactMinDomains, out TLDProductTier dotTypeTier)
    {
      return _tierGroups.TryGetValue(exactMinDomains, out dotTypeTier);
    }

  }
}
