﻿using System;

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

    public int GetProductId(int registrionLength)
    {
      int result = 0;
      if ((registrionLength >= 1) && (registrionLength <= _productIdsByYearsMinusOne.Length))
      {
        int index = registrionLength - 1;
        result = _productIdsByYearsMinusOne[index];
      }
      return result;
    }

    public bool IsLengthValid(int registrationLength)
    {
      int productId = GetProductId(registrationLength);
      return (productId != 0);
    }

    public void AddProductId(int registrionLength, int productId)
    {
      this._productIdsByYearsMinusOne[registrionLength - 1] = productId;
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
