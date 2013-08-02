using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Geo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  public class StateNamesTests
  {
    [TestMethod]
    public void StateNamesRequestDataProperties()
    {
      var request = new StateNamesRequestData("en-US", 226);
      Assert.AreEqual("en-us", request.FullLanguage);
    }

    [TestMethod]
    public void StateNamesRequestDataCacheKey()
    {
      var request = new StateNamesRequestData("en-US", 226);
      var request2 = new StateNamesRequestData("en-us", 226);
      var request3 = new StateNamesRequestData("fr-CA", 226);

      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request.GetCacheMD5(), request3.GetCacheMD5());
    }

    [TestMethod]
    public void StateNamesRequestDataNull()
    {
      var request = new StateNamesRequestData(null, 226);
      Assert.AreEqual(string.Empty, request.FullLanguage);
      Assert.AreEqual(":226", request.GetCacheMD5());
    }

    [TestMethod]
    public void StateNamesResponseDataEmpty()
    {
      var response = StateNamesResponseData.Empty;
      Assert.AreEqual(null, response.GetException());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void StateNameResponseDataFromGoodXml()
    {
      const string xml =
        "<LocaleData lang=\"pt-br\"><StateList><State id=\"74\" name=\"Alabama\"/><State id=\"75\" name=\"Alasca\"/><State id=\"78\" name=\"Arizona\"/></StateList></LocaleData>";
      var response = StateNamesResponseData.FromServiceData(xml);

      Assert.AreNotEqual(0, response.Count);

      string name;
      var found = response.TryGetNameById(74, out name);
      Assert.IsTrue(found);
      Assert.IsNotNull(name);

      found = response.TryGetNameById(9999, out name);
      Assert.IsFalse(found);

      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void StateNameResponseDataFromMissingData()
    {
      const string xml =
        "<LocaleData lang=\"pt-br\"><StateList><State id=\"74\" name=\"Alabama\"/><State id=\"75\" /><State name=\"Arizona\"/></StateList></LocaleData>";
      var response = StateNamesResponseData.FromServiceData(xml);

      Assert.AreEqual(1, response.Count);

      string name;
      var found = response.TryGetNameById(75, out name);
      Assert.IsFalse(found);
    }


    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void StateNameResponseDataFromBadXml()
    {
      const string xml =
        "<LocaleData lang=\"pt-br\"<State id=\"74\" name=\"Alabama\"/><State id=\"75\" name=\"Alasca\"/><State id=\"78\" name=\"Arizona\"/></StateList></LocaleData>";
      var response = StateNamesResponseData.FromServiceData(xml);
    }

    private const int _REQUESTTYPE = 723;

    [TestMethod]
    public void StateNamesServiceCallEn()
    {
      var request = new StateNamesRequestData("en-US", 226);
      var response = (StateNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void StateNamesServiceCallPt()
    {
      var request = new StateNamesRequestData("pt-BR", 226);
      var response = (StateNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallEmptyLanguage()
    {
      var request = new StateNamesRequestData("qa-qa", 226);
      var response = (StateNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CountryNamesServiceCallEmptyCountry()
    {
      var request = new StateNamesRequestData("en-us", 444);
      var response = (StateNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.Count);
    }


  }

}
