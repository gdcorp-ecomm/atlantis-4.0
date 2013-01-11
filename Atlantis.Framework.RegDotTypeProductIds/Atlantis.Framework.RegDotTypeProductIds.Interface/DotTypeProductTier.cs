using System;
using System.Collections.Generic;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  internal class DotTypeProductTier : IComparable<DotTypeProductTier>
  {
    private int _minDomainCount = 0;
    private Dictionary<int, DotTypeProduct> _productsByYear;
 
    internal DotTypeProductTier(int minDomains)
    {
      _minDomainCount = minDomains;
      _productsByYear = new Dictionary<int, DotTypeProduct>(10);
    }

    public int MinDomainCount
    {
      get { return _minDomainCount; }
    }

    public bool TryGetProduct(int years, out DotTypeProduct product)
    {
      return _productsByYear.TryGetValue(years, out product);
    }

    internal void AddProduct(DotTypeProduct product)
    {
      _productsByYear[product.Years] = product;
    }

    public int CompareTo(DotTypeProductTier other)
    {
      if (other == null)
      {
        return -1;
      }
      else
      {
        return this.MinDomainCount.CompareTo(other.MinDomainCount);
      }
    }
  }
}
