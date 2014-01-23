using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Tests.Properties;
using Newtonsoft.Json;
using Atlantis.Framework.Products.Impl;
using Atlantis.Framework.Products.Interface;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupsOfferedMarketsRequestTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedMarketsRequest_ConstructorTest()
    {
      var target = new ProductGroupsOfferedMarketsRequest();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedMarketsRequest_RequestHandlerTest()
    {
      var target = new ProductGroupsOfferedMarketsRequest();
      Assert.IsNotNull(target);

      const string assembly = "Atlantis.Framework.CDS.Impl.dll";
      var progId = String.Empty;
      const int requestType = 793;
      var config = new WsConfigElement(requestType, progId, assembly, "http://cms.dev.glbt1.gdg/");
      RequestData requestData = new ProductGroupsOfferedMarketsRequestData();
      var actual = target.RequestHandler(requestData, config);

      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupsOfferedMarketsResponseData));

      var data = Resources.ProductGroupsMarketsJson;
      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeAnonymousType(data, new
        {
          Content = string.Empty
        });
        var privates = new PrivateObject(actual);
        var prodGroups = privates.GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        foreach (var item in items.Where((i) => "99".Equals(i.Attribute("id").Value)))
        {
          CollectionAssert.Contains(prodGroups.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets.Where((m) => !"es-XX".Equals(m.Attribute("id").Value, StringComparison.OrdinalIgnoreCase)))
          {
            Assert.IsTrue(prodGroups[int.Parse(item.Attribute("id").Value)].ContainsMarket(market.Attribute("id").Value));
          }
        }
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
    public void ProductGroupOfferedMarketsRequest_IntegrationTest()
    {
      const int _REQUESTTYPE = 793;
      var request = new ProductGroupsOfferedMarketsRequestData();
      var actual = Engine.Engine.ProcessRequest(request, _REQUESTTYPE) as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(actual);

      var data = Resources.ProductGroupsMarketsJson;

      if (!string.IsNullOrEmpty(data))
      {
        var contentVersion = JsonConvert.DeserializeAnonymousType(data, new
        {
          Content = string.Empty
        });
        var privates = new PrivateObject(actual);
        var prodGroups = privates.GetField("_productGroups") as IDictionary<int, ProductGroupMarketData>;
        var items = XElement.Parse(contentVersion.Content).Descendants("productGroup");
        foreach (var item in items.Where((i) => "99".Equals(i.Attribute("id").Value)))
        {
          CollectionAssert.Contains(prodGroups.Keys.ToList(), int.Parse(item.Attribute("id").Value));
          var markets = item.Descendants("markets").Descendants("market");
          foreach (var market in markets.Where((m) => !"es-XX".Equals(m.Attribute("id").Value, StringComparison.OrdinalIgnoreCase)))
          {
            Assert.IsTrue(prodGroups[int.Parse(item.Attribute("id").Value)].ContainsMarket(market.Attribute("id").Value));
          }
        }
      }
    }
  }
}
