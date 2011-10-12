using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public sealed class DotTypeProductIds
  {
    private DotTypeProductIdTypes _productIdType;
    private List<DotTypeTier> _tierGroups;

    public DotTypeProductIdTypes ProductIdType
    {
      get { return _productIdType; }
    }

    public DotTypeProductIds(DotTypeProductIdTypes productIdType, IEnumerable<DotTypeTier> tiers)
    {
      _productIdType = productIdType;
      _tierGroups = new List<DotTypeTier>(tiers);
      _tierGroups.Sort();
    }

    public bool IsLengthValid(int registrationLength, int domainCount)
    {
      bool result = false;

      DotTypeTier tierGroup = GetTier(domainCount);

      if (tierGroup != null)
      {
        result = tierGroup.IsLengthValid(registrationLength);
      }

      return result;
    }

    public int GetProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      DotTypeTier tierGroup = GetTier(domainCount);

      if (tierGroup != null)
      {
        result = tierGroup.GetProductId(registrationLength);
      }

      return result;
    }

    public void AddDotTypeTier(DotTypeTier dotTypeTier)
    {
      this._tierGroups.Add(dotTypeTier);
      _tierGroups.Sort();
    }

    public DotTypeTier GetTier(int domainCount)
    {
      DotTypeTier result = null;

      foreach (DotTypeTier tier in _tierGroups)
      {
        if (domainCount >= tier.MinDomains)
        {
          result = tier;
        }
        else
        {
          break;
        }
      }

      if ((result == null) && (_tierGroups.Count > 0))
      {
        result = _tierGroups[0];
      }

      return result;
    }
  }
}
