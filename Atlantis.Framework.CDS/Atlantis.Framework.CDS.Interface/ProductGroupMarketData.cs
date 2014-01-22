using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Atlantis.Framework.CDS.Tests")]
namespace Atlantis.Framework.CDS.Interface
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

