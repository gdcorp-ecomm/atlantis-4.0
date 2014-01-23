using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Products.Interface;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Products.Impl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Tests.Properties;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupOfferedCountriesResponseDataTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedResponseData_ConstructorTest()
    {
      string data = Resources.ProductGroupCountries;
      var privates = new PrivateObject(typeof(ProductGroupOfferedCountriesResponseData), new[] { data });
      var target = privates.Target as ProductGroupOfferedCountriesResponseData;
      Assert.IsNotNull(target);

      var actual = privates.GetField("_operatingCompaniesByCountry") as IDictionary<string, OperatingCompany>;
      Assert.IsNotNull(actual);

      if (!ReferenceEquals(null, data))
      {
        var items = XElement.Parse(data).Descendants("item");
        foreach (var item in items)
        {
          CollectionAssert.Contains(actual.Keys.ToList(), item.Attribute("countryCode").Value);
        }
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_ContainsCountryTest()
    {
      string data = Resources.ProductGroupCountries;
      var privates = new PrivateObject(typeof(ProductGroupOfferedCountriesResponseData), new[] { data });
      var target = privates.Target as ProductGroupOfferedCountriesResponseData;
      Assert.IsNotNull(target);

      var actual = target.ContainsCountry("us");
      var items = XElement.Parse(data).Descendants("item");

      foreach (var item in items)
      {
        Assert.IsTrue(target.ContainsCountry(item.Attribute("countryCode").Value));
      }

    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductOfferedCountriesResponseData_TryGetOperatingCompanyTest()
    {
      string data = Resources.ProductGroupCountries;
      var privates = new PrivateObject(typeof(ProductGroupOfferedCountriesResponseData), new[] { data });
      var target = privates.Target as ProductGroupOfferedCountriesResponseData;
      Assert.IsNotNull(target);

      var items = XElement.Parse(data).Descendants("item");
      foreach (var item in items)
      {
        OperatingCompany company = null;
        bool actual = target.TryGetOperatingCompany(item.Attribute("countryCode").Value, out company);
        Assert.IsTrue(actual);
        Assert.AreEqual(item.Attribute("msft_operatingCompanyID").Value, company.CompanyId);
      }
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
    public void ProductOfferedResponseData_FromDataTest()
    {
      string data = Resources.ProductGroupCountries;
      var actual = ProductGroupOfferedCountriesResponseData.FromCacheData(data);
      Assert.IsNotNull(actual);
      Assert.IsInstanceOfType(actual, typeof(ProductGroupOfferedCountriesResponseData));

      var countries = new PrivateObject(actual).GetField("_operatingCompaniesByCountry") as IDictionary<string, OperatingCompany>;
      Assert.IsNotNull(countries);

      if (!ReferenceEquals(null, data))
      {
        var items = XElement.Parse(data).Descendants("item");
        Assert.AreEqual(items.Count(), actual.Count);
        foreach (var item in items)
        {
          CollectionAssert.Contains(countries.Keys.ToList(), item.Attribute("countryCode").Value);
        }
      }
    }
  }
}
