using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public sealed class StaticDotTypeTiers
  {
    private StaticDotTypeProductIdTypes _productIdType;
    private List<StaticDotTypeTier> _tierGroups;

    public StaticDotTypeProductIdTypes ProductIdType
    {
      get { return _productIdType; }
    }

    public StaticDotTypeTiers(StaticDotTypeProductIdTypes productIdType, IEnumerable<StaticDotTypeTier> tiers)
    {
      _productIdType = productIdType;
      _tierGroups = new List<StaticDotTypeTier>(tiers);
      _tierGroups.Sort();
    }

    public bool IsLengthValid(int registrationLength, int domainCount)
    {
      bool result = false;

      StaticDotTypeTier tierGroup = GetTier(domainCount);

      if (tierGroup != null)
      {
        result = tierGroup.IsLengthValid(registrationLength);
      }

      return result;
    }

    public int GetProductId(int registrationLength, int domainCount)
    {
      int result = 0;

      StaticDotTypeTier tierGroup = GetTier(domainCount);

      if (tierGroup != null)
      {
        result = tierGroup.GetProductId(registrationLength);
      }

      return result;
    }

    public void AddDotTypeTier(StaticDotTypeTier dotTypeTier)
    {
      this._tierGroups.Add(dotTypeTier);
      _tierGroups.Sort();
    }

    public StaticDotTypeTier GetTier(int domainCount)
    {
      StaticDotTypeTier result = null;

      foreach (StaticDotTypeTier tier in _tierGroups)
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
