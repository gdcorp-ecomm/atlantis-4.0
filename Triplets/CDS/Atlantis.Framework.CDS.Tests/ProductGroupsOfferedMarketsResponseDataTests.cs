using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Interface;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.CDS.Impl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CDS.Tests.Properties;
using System.Linq;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Tests
{
  [TestClass]
  public class ProductGroupsOfferedMarketsResponseDataTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedResponseData_ConstructorTest()
    {
      string data = Resources.ProductGroupMarketsJson;

      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      var actual = privates.GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
      Assert.IsNotNull(actual);

      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeObject<ContentVersion>(data);
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        foreach (var item in items)
        {
          CollectionAssert.Contains(actual.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets)
          {
            CollectionAssert.Contains(actual[int.Parse(item.Attribute("id").Value)].Markets.ToList(), market.Attribute("id").Value);
          }
        }
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_ContainsMarketTest()
    {
      string data = Resources.ProductGroupMarketsJson;
      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      var actual = target.ContainsMarket(99, "en-us");
      Assert.IsTrue(actual);

      var contentVersion = JsonConvert.DeserializeObject<ContentVersion>(data);
      var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");

      foreach (var item in items)
      {
        var markets = item.Descendants("markets").Descendants("market");
        foreach (var market in markets)
        {
          Assert.IsTrue(target.ContainsMarket(99, market.Attribute("id").Value));
        }
      }

    }
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_ContainsMarketFalseTest()
    {
      string data = Resources.ProductGroupMarketsJson;
      var privates = new PrivateObject(typeof(ProductGroupsOfferedMarketsResponseData), new[] { data });
      var target = privates.Target as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(target);

      var actual = target.ContainsMarket(101, "en-us");
      Assert.IsFalse(actual);

      actual = target.ContainsMarket(99, "en-CB");
      Assert.IsFalse(actual);

    }


    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedResponseData_FromCDSResponse()
    {
      string data = Resources.ProductGroupMarketsJson;
      var actual = ProductGroupsOfferedMarketsResponseData.FromCDSResponse(data);
      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupsOfferedMarketsResponseData));

      var productGroupMarkets = new PrivateObject(actual).GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
      Assert.IsNotNull(productGroupMarkets);

      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeObject<ContentVersion>(data);
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        Assert.AreEqual(2, actual.Count);
        foreach (var item in items)
        {
          CollectionAssert.Contains(productGroupMarkets.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets)
          {
            CollectionAssert.Contains(productGroupMarkets[int.Parse(item.Attribute("id").Value)].Markets.ToList(), market.Attribute("id").Value);
          }
        }
      }

      //if (!ReferenceEquals(null, data))
      //{
      //  var items = XElement.Parse(data).Descendants("item");
      //  Assert.AreEqual(items.Count(), actual.Count);
      //  foreach (var item in items)
      //  {
      //    CollectionAssert.Contains(productGroupMarkets.Keys.ToList(), item.Attribute("countryCode").Value);
      //  }
      //}
    }
  }
}
