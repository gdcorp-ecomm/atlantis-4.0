using System.Collections.Generic;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  public class DotTypeProductTiers
  {
    private DotTypeProductTypes _productType;
    private SortedList<int, DotTypeProductTier> _tierGroups;

    public DotTypeProductTypes ProductType
    {
      get { return _productType; }
    }

    internal DotTypeProductTiers(DotTypeProductTypes productType)
    {
      _productType = productType;
      _tierGroups = new SortedList<int, DotTypeProductTier>(5);
    }

    public bool TryGetProduct(int years, int domainCount, out DotTypeProduct product)
    {
      product = null;
      bool result = false;

      DotTypeProductTier tier = GetValidTier(domainCount);
      if (tier != null)
      {
        result = tier.TryGetProduct(years, out product);
      }

      return result;
    }

    private DotTypeProductTier GetValidTier(int domainCount)
    {
      DotTypeProductTier result = null;

      foreach(var tierPair in _tierGroups)
      {
        if ((domainCount >= tierPair.Key) || (result == null))
        {
          result = tierPair.Value;
        }
        else
        {
          break;
        }
      }

      return result;
    }

    internal void AddDotTypeTier(DotTypeProductTier dotTypeTier)
    {
      _tierGroups.Add(dotTypeTier.MinDomainCount, dotTypeTier);
    }

    internal bool TryGetTier(int exactMinDomains, out DotTypeProductTier dotTypeTier)
    {
      return _tierGroups.TryGetValue(exactMinDomains, out dotTypeTier);
    }

  }
}
