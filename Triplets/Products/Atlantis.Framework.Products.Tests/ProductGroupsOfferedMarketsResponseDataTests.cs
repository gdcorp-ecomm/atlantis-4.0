using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Products.Tests.Properties;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupsOfferedMarketsResponseDataTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedResponseData_ConstructorTest()
    {
      var data = Resources.ProductGroupsMarketsJson;

      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      var actual = privates.GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
      Assert.IsNotNull(actual);

      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeAnonymousType(data, new
        {
          Content = string.Empty
        });
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        foreach (var item in items)
        {
          CollectionAssert.Contains(actual.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets)
          {
            Assert.IsTrue(actual[int.Parse(item.Attribute("id").Value)].ContainsMarket(market.Attribute("id").Value));
          }
        }
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_TryGetMarketDataTest()
    {
      var data = Resources.ProductGroupsMarketsJson;
      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      var actualMrktData = (ProductGroupMarketData)null;
      var actual = target.TryGetMarketData(99, out actualMrktData);
      Assert.IsTrue(actual);
      Assert.IsNotNull(actualMrktData);

      var contentVersion = JsonConvert.DeserializeAnonymousType(data, new
      {
        Content = string.Empty
      });
      var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
      foreach (var item in items)
      {
        var markets = item.Descendants("markets").Descendants("market");
        foreach (var market in markets)
        {
          Assert.IsTrue(actualMrktData.ContainsMarket(market.Attribute("id").Value));
        }
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_TryGetMarketDataFailTest()
    {
      var data = Resources.ProductGroupsMarketsJson;
      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      ProductGroupMarketData actualMktData;
      var actual = target.TryGetMarketData(101, out actualMktData);
      Assert.IsFalse(actual);
    }


    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedResponseData_FromCDSResponse()
    {
      var data = Resources.ProductGroupsMarketsJson;
      var actual = ProductGroupsOfferedMarketsResponseData.FromCDSResponse(data);
      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupsOfferedMarketsResponseData));

      var productGroupMarkets = new PrivateObject(actual).GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
      Assert.IsNotNull(productGroupMarkets);

      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeAnonymousType(data, new
        {
          Content = string.Empty
        });
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        Assert.AreEqual(2, actual.Count);
        foreach (var item in items)
        {
          CollectionAssert.Contains(productGroupMarkets.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets)
          {
            Assert.IsTrue(productGroupMarkets[int.Parse(item.Attribute("id").Value)].ContainsMarket(market.Attribute("id").Value));
          }
        }
      }
    }
  }
}
