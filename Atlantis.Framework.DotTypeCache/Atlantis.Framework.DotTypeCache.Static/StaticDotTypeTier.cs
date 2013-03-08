using System;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public sealed class StaticDotTypeTier : IComparable<StaticDotTypeTier>
  {
    private int _minDomains = 0;
    private int[] _productIdsByYearsMinusOne;

    public StaticDotTypeTier(int minDomains, int[] productIds)
    {
      _minDomains = minDomains;
      _productIdsByYearsMinusOne = productIds;
    }

    public int MinDomains
    {
      get { return _minDomains; }
    }

    public int GetProductId(int registrationLength)
    {
      int result = 0;
      if ((registrationLength >= 1) && (registrationLength <= _productIdsByYearsMinusOne.Length))
      {
        int index = registrationLength - 1;
        result = _productIdsByYearsMinusOne[index];
      }
      return result;
    }

    public bool IsLengthValid(int registrationLength)
    {
      int productId = GetProductId(registrationLength);
      return (productId != 0);
    }

    public void AddProductId(int registrationLength, int productId)
    {
      this._productIdsByYearsMinusOne[registrationLength - 1] = productId;
    }

    #region IComparable<Tier> Members

    public int CompareTo(StaticDotTypeTier other)
    {
      if (other == null)
      {
        return -1;
      }
      else
      {
        return this.MinDomains.CompareTo(other.MinDomains);
      }
    }

    #endregion
  }
}
