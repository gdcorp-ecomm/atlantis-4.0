using Atlantis.Framework.Interface;
using System;
using System.Globalization;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupOfferedCountriesRequestData : RequestData
  {
    public ProductGroupOfferedCountriesRequestData(int productGroupId)
    {
      ProductGroupId = productGroupId;
    }
    public int ProductGroupId { get;
      private set; }

    public override string GetCacheMD5()
    {
      return ProductGroupId.ToString(CultureInfo.InvariantCulture);
    }
  }
}
