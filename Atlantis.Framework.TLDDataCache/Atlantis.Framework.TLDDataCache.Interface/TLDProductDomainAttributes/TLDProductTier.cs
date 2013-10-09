using System;
using System.Collections.Generic;

namespace Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes
{
  internal class TLDProductTier : IComparable<TLDProductTier>
  {
    private readonly int _minDomainCount;
    private readonly Dictionary<int, TLDProduct> _productsByYear;

    internal TLDProductTier(int minDomains)
    {
      _minDomainCount = minDomains;
      _productsByYear = new Dictionary<int, TLDProduct>(10);
    }

    public int MinDomainCount
    {
      get { return _minDomainCount; }
    }

    public bool TryGetProduct(int years, out TLDProduct product)
    {
      return _productsByYear.TryGetValue(years, out product);
    }

    internal void AddProduct(TLDProduct product)
    {
      _productsByYear[product.Years] = product;
    }

    public int CompareTo(TLDProductTier other)
    {
      if (other == null)
      {
        return -1;
      }

      return MinDomainCount.CompareTo(other.MinDomainCount);
    }
  }
}
