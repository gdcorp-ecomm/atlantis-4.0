using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Products.Interface;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Products.Impl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Tests.Properties;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupOfferedCountriesRequestTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedCountriesRequest_ConstructorTest()
    {
      var target = new ProductGroupOfferedCountriesRequest();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedCountriesRequest_RequestHandlerTest()
    {
      var target = new ProductGroupOfferedCountriesRequest();
      Assert.IsNotNull(target);

      const string assembly = "Atlantis.Framework.Products.Impl.dll";
      string progId = String.Empty;
      const int requestType = 792;
      ConfigElement config = new ConfigElement(requestType, progId, assembly);
      int productGroupId = 99;
      RequestData requestData = new ProductGroupOfferedCountriesRequestData(productGroupId);
      var actual = target.RequestHandler(requestData, config);

      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupOfferedCountriesResponseData));

    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
    public void ProductGroupOfferedCountriesRequest_IntegrationTest()
    {
      const int productGroupId = 99;
      const int _REQUESTTYPE = 792;
      var request = new ProductGroupOfferedCountriesRequestData(productGroupId);
      var actual = Engine.Engine.ProcessRequest(request, _REQUESTTYPE) as ProductGroupOfferedCountriesResponseData;
      Assert.IsNotNull(actual);

      string expectedData = Resources.ProductGroupCountries;

      var items = XElement.Parse(expectedData).Descendants("item");
      var countries = new PrivateObject(actual).GetField("_operatingCompaniesByCountry") as IDictionary<string, OperatingCompany>;
      Assert.AreEqual(items.Count(), countries.Count);
      foreach (var item in items)
      {
        CollectionAssert.Contains(countries.Keys.ToList(), item.Attribute("countryCode").Value);
      }
    }
  }
}
