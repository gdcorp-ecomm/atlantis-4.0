using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupsOfferedMarketsResponseData : ResponseData
  {
    private Dictionary<int, ProductGroupMarketData> _productGroups = new Dictionary<int, ProductGroupMarketData>();

    public static ProductGroupsOfferedMarketsResponseData NotFound
    {
      get;
      private set;
    }

    static ProductGroupsOfferedMarketsResponseData()
    {
      NotFound = new ProductGroupsOfferedMarketsResponseData(string.Empty);
    }

    public static ProductGroupsOfferedMarketsResponseData FromCDSResponse(string responseData)
    {
      return new ProductGroupsOfferedMarketsResponseData(responseData);
    }

    private ProductGroupsOfferedMarketsResponseData(string responseData)
    {
      if (!string.IsNullOrEmpty(responseData))
      {
        var contentVersion = JsonConvert.DeserializeAnonymousType(responseData, new
        {
          Content = string.Empty
        });

        if (!string.IsNullOrEmpty(contentVersion.Content))
        {
          var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
          var productGroupId = 0;
          ProductGroupMarketData temp = null;
          foreach (var item in items)
          {
            if (int.TryParse(item.Attribute("id").Value, out productGroupId))
            {
              temp = _productGroups.ContainsKey(productGroupId) ? _productGroups[productGroupId] : new ProductGroupMarketData(productGroupId);
              var markets = item.Descendants("markets").Descendants("market");
              foreach (var market in markets)
              {
                temp.AddMarket(market.Attribute("id").Value);
              }
              _productGroups[productGroupId] = temp;
            }
          }
        }
      }
    }

    public int Count
    {
      get
      {
        return _productGroups.Count;
      }
    }

    public bool TryGetMarketData(int productGroupId, out ProductGroupMarketData marketData)
    {
      return _productGroups.TryGetValue(productGroupId, out marketData);
    }
  }
}
