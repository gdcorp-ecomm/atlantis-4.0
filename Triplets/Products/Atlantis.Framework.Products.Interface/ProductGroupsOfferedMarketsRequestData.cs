using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupsOfferedMarketsRequestData : RequestData
  {
    public override string GetCacheMD5()
    {
      return "data";
    }
  }
}

