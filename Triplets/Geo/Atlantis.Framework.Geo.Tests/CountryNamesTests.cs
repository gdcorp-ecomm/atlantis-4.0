using System;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Geo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  public class CountryNamesTests
  {
    [TestMethod]
    public void CountryNamesRequestDataProperties()
    {
      var request = new CountryNamesRequestData("en-US");
      Assert.AreEqual("en-us", request.FullLanguage);
    }

    [TestMethod]
    public void CountryNamesRequestDataCacheKey()
    {
      var request = new CountryNamesRequestData("en-US");
      var request2 = new CountryNamesRequestData("en-us");
      var request3 = new CountryNamesRequestData("fr-CA");

      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request.GetCacheMD5(), request3.GetCacheMD5());
    }

    [TestMethod]
    public void CountryNamesRequestDataNull()
    {
      var request = new CountryNamesRequestData(null);
      Assert.AreEqual(string.Empty, request.FullLanguage);
      Assert.AreEqual(string.Empty, request.GetCacheMD5());
    }

    [TestMethod]
    public void CountryNamesResponseDataEmpty()
    {
      var response = CountryNamesResponseData.Empty;
      Assert.AreEqual(null, response.GetException());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void CountryNameResponseDataFromGoodXml()
    {
      const string xml =
        "<LocaleData><Item id=\"226\" country=\"Estados Unidos\"/><Item id=\"102\" country=\"Índia\"/><Item id=\"111\" country=\"Japão\"/><Item id=\"172\" country=\"Filipinas\"/></LocaleData>";
      var response = CountryNamesResponseData.FromServiceData(xml);

      Assert.AreNotEqual(0, response.Count);

      string name;
      var found = response.TryGetNameById(226, out name);
      Assert.IsTrue(found);
      Assert.IsNotNull(name);

      found = response.TryGetNameById(9999, out name);
      Assert.IsFalse(found);

      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void CountryNameResponseDataFromMissingData()
    {
      const string xml =
        "<LocaleData lang=\"pt-br\"><Item id=\"226\" country=\"Estados Unidos\"/><Item id=\"102\" /><Item country=\"Japão\"/><Item id=\"172\" country=\"Filipinas\"/></LocaleData>";
      var response = CountryNamesResponseData.FromServiceData(xml);

      Assert.AreEqual(2, response.Count);

      string name;
      var found = response.TryGetNameById(102, out name);
      Assert.IsFalse(found);
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void CountryNameResponseDataFromBadXml()
    {
      const string xml =
        "<LocaleData lang=\"pt-br\"<Item id=\"226\" country=\"Estados Unidos\"/><Item id=\"102\" country=\"Índia\"/><Item id=\"111\" country=\"Japão\"/><Item id=\"172\" country=\"Filipinas\"/></LocaleData>";
      var response = CountryNamesResponseData.FromServiceData(xml);
    }

    private const int _REQUESTTYPE = 722;

    [TestMethod]
    public void CountryNamesServiceCallEn()
    {
      var request = new CountryNamesRequestData("en-US");
      var response = (CountryNamesResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallPt()
    {
      var request = new CountryNamesRequestData("pt-BR");
      var response = (CountryNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallInvalidLanguageReturnsEnglish()
    {
      var request = new CountryNamesRequestData("qa-unit");
      var response = (CountryNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallNullLanguage()
    {
      var request = new CountryNamesRequestData(null);
      var response = (CountryNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallEmptyLanguage()
    {
      var request = new CountryNamesRequestData(string.Empty);
      var response = (CountryNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.Count);
    }

  }
}
