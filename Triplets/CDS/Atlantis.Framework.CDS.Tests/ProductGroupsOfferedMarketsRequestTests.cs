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
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Tests
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
      string progId = String.Empty;
      const int requestType = 800;
      var config = new WsConfigElement(requestType, progId, assembly, "http://cms.dev.glbt1.gdg/");
      int productGroupId = 99;
      const string query = "content/productgroup/availablemarkets";
      RequestData requestData = new CDSRequestData(query);
      var actual = target.RequestHandler(requestData, config);

      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupsOfferedMarketsResponseData));

    }

  //  [TestMethod]
  //  [ExcludeFromCodeCoverage]
  //  public void ProductGroupOfferedMarketsRequest_RequestHandlerMissing
  //{

  //}

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
    public void ProductGroupOfferedMarketsRequest_IntegrationTest()
    {
      const int productGroupId = 99;
      const int _REQUESTTYPE = 800;
      const string query = "content/productgroup/availablemarkets";
      var request = new CDSRequestData(query);
      var actual = Engine.Engine.ProcessRequest(request, _REQUESTTYPE) as ProductGroupsOfferedMarketsResponseData;
      Assert.IsNotNull(actual);

      string expectedData = Resources.ProductGroupMarketsJson;
      var contentVersion = JsonConvert.DeserializeObject<ContentVersion>(expectedData);
      //var items = XElement.Parse(expectedData).Descendants("item");
      //var countries = new PrivateObject(actual).GetField("_operatingCompaniesByCountry") as IDictionary<string, OperatingCompany>;
      //Assert.AreEqual(items.Count(), countries.Count);
      //foreach (var item in items)
      //{
      //  CollectionAssert.Contains(countries.Keys.ToList(), item.Attribute("countryCode").Value);
      //}

    }
  }
}
