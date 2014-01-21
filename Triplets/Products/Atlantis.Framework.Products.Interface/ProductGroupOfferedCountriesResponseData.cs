using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupOfferedCountriesResponseData : ResponseData
  {
    private IDictionary<string, OperatingCompany> _operatingCompaniesByCountry;
    public static ProductGroupOfferedCountriesResponseData FromCacheData(string cacheData)
    {
      return new ProductGroupOfferedCountriesResponseData(cacheData);
    }

    private ProductGroupOfferedCountriesResponseData(string cacheData)
    {
      _operatingCompaniesByCountry = new Dictionary<string, OperatingCompany>(StringComparer.OrdinalIgnoreCase);
      if (!ReferenceEquals(null, cacheData))
      {
        var items = XElement.Parse(cacheData).Descendants("item");
        var country = string.Empty;
        foreach (var item in items)
        {
          country = item.Attribute("countryCode").Value;
          _operatingCompaniesByCountry[country] = new OperatingCompany(item.Attribute("msft_operatingCompanyID").Value, country);
        }
      }
      Count = _operatingCompaniesByCountry.Count;
    }

    public int Count
    {
      get;
      private set;
    }

    public bool ContainsCountry(string countryCode)
    {
      return _operatingCompaniesByCountry.Keys.Contains(countryCode);
    }

    public bool TryGetOperatingCompany(string countryCode, out OperatingCompany company)
    {
      return _operatingCompaniesByCountry.TryGetValue(countryCode, out company);
    }
  }
}
