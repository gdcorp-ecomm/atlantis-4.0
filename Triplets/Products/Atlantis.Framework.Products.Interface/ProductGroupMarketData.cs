using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Atlantis.Framework.Products.Tests")]
namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupMarketData
  {
    private HashSet<string> _markets;
    public ProductGroupMarketData(int productGroupId)
    {
      ProductGroupId = productGroupId;
      _markets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    internal void AddMarket(string marketId)
    {
      _markets.Add(marketId);
    }

    public bool ContainsMarket(string marketId)
    {
      return _markets.Contains(marketId);
    }

    public int ProductGroupId
    {
      get;
      private set;
    }
  }
}

