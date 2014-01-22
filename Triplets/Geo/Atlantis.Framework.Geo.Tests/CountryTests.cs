using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  public class CountryTests
  {
    const int _COUNTRYREQUESTTYPE = 664;

    [TestMethod]
    public void CreateCountryObject()
    {
      string cacheXml = "<country id=\"226\" code=\"us\" name=\"United States\" callingcode=\"1\" supported=\"1\" />";
      XElement cacheElement = XElement.Parse(cacheXml);
      Country country = Country.FromCacheElement(cacheElement);
      Assert.IsNotNull(country);
      Assert.AreEqual(226, country.Id);
      Assert.AreEqual("us", country.Code);
      Assert.AreEqual("United States", country.Name);
      Assert.AreEqual("1", country.CallingCode);
    }

    [TestMethod]
    public void CountryRequestCacheKey()
    {
      CountryRequestData request = new CountryRequestData();
      CountryRequestData request2 = new CountryRequestData();
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CountryRequestXml()
    {
      CountryRequestData request = new CountryRequestData();
      string xml = request.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void CountryResponseException()
    {
      AtlantisException exception = new AtlantisException("CountryTests.CountryResponseException", 0, "TestMessage", "TestData");
      CountryResponseData response = CountryResponseData.FromException(exception);
      Assert.IsNotNull(response.GetException());
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void CountryResponseBadXml()
    {
      string cacheXml = "<country id=\"226\" code=\"us\" name=\"United States\" callingcode=\"1\" supported=\"1\"";
      CountryResponseData response = CountryResponseData.FromDataCacheXml(cacheXml);
    }

    [TestMethod]
    public void CountryResponseValid()
    {
      string cacheXml = "<countries><country id=\"226\" code=\"us\" name=\"United States\" callingcode=\"1\" supported=\"1\" /></countries>";
      CountryResponseData response = CountryResponseData.FromDataCacheXml(cacheXml);
      Assert.AreNotEqual(0, response.Countries.Count());
    }

    [TestMethod]
    public void GetCountries()
    {
      CountryRequestData request = new CountryRequestData();
      CountryResponseData response = (CountryResponseData)Engine.Engine.ProcessRequest(request, _COUNTRYREQUESTTYPE);
      Assert.AreNotEqual(0, response.Countries.Count());

      string xml = response.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void GetCountryByName()
    {
      CountryRequestData request = new CountryRequestData();
      CountryResponseData response = (CountryResponseData)Engine.Engine.ProcessRequest(request, _COUNTRYREQUESTTYPE);
      Country country = response.FindCountryByName("UNITED STATES");
      Assert.IsNotNull(country);
    }

    [TestMethod]
    public void GetCountryById()
    {
      CountryRequestData request = new CountryRequestData();
      CountryResponseData response = (CountryResponseData)Engine.Engine.ProcessRequest(request, _COUNTRYREQUESTTYPE);
      Country country = response.FindCountryById(226);
      Assert.IsNotNull(country);
    }

    [TestMethod]
    public void GetCountryByCode()
    {
      CountryRequestData request = new CountryRequestData();
      CountryResponseData response = (CountryResponseData)Engine.Engine.ProcessRequest(request, _COUNTRYREQUESTTYPE);
      Country country = response.FindCountryByCode("us");
      Assert.IsNotNull(country);

      Country country2 = response.FindCountryByCode("Us");
      Assert.AreEqual(country.Id, country2.Id, "Different case incorrectly returned different countries.");
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void GetCountriesExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (CountryResponseData)Engine.Engine.ProcessRequest(request, _COUNTRYREQUESTTYPE);
    }

  }
}
