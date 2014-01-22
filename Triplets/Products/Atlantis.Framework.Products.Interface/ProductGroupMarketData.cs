using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Atlantis.Framework.Products.Tests")]
namespace Atlantis.Framework.Products.Interface
{
  internal class ProductGroupMarketData
  {
    public ProductGroupMarketData(int productGroupId)
    {
      ProductGroupId = productGroupId;
      Markets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      ;
    }

    public HashSet<string> Markets
    {
      get;
      private set;
    }

    public int ProductGroupId
    {
      get;
      private set;
    }
  }
}

