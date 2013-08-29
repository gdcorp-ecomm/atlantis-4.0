using System;
using System.Xml;
using Atlantis.Framework.Products.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductNamesTests
  {
    [TestMethod]
    public void ProductNamesRequestDataProperties()
    {
      var request = new ProductNamesRequestData("en-US", 101);
      Assert.AreEqual("en-us", request.FullLanguage);
      Assert.AreEqual(101, request.NonUnifiedPfid);
    }

    [TestMethod]
    public void ProductNamesRequestDataCacheKey()
    {
      var request = new ProductNamesRequestData("en-US", 101);
      var request2 = new ProductNamesRequestData("en-us", 101);
      var request3 = new ProductNamesRequestData("en-US", 220101);
      var request4 = new ProductNamesRequestData("pt-BR", 101);

      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request.GetCacheMD5(), request3.GetCacheMD5());
      Assert.AreNotEqual(request2.GetCacheMD5(), request4.GetCacheMD5());
    }

    [TestMethod]
    public void ProductNamesRequestDataNullLanguage()
    {
      var request = new ProductNamesRequestData(null, 101);
      Assert.AreEqual(string.Empty, request.FullLanguage);
      Assert.AreEqual(101, request.NonUnifiedPfid);
    }

    [TestMethod]
    public void ProductNamesResponseDataProperties()
    {
      string data = "<LocaleData name=\".COM Domain Name Registration - 1 Ano (recorrente)\" description2=\".COM Registo de Domínios\"/>";
      var response = ProductNamesResponseData.FromServiceData(data);
      Assert.AreEqual(".COM Domain Name Registration - 1 Ano (recorrente)", response.Name);
      Assert.AreEqual(".COM Registo de Domínios", response.FriendlyName);
      XElement.Parse(response.ToXML());
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void ProductNamesResponseDataBadXml()
    {
      string data = "<LocaleData lang=\"pt-br\"Product pfid=\"101\" name=\".COM Domain Name Registration - 1 Ano (recorrente)\" description2=\".COM Registo de Domínios\"/></LocaleData>";
      var response = ProductNamesResponseData.FromServiceData(data);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void ProductNamesResponseDataMissingAttributes()
    {
      string data = "<LocaleData name=\".COM Domain Name Registration - 1 Ano (recorrente)\" />";
      var response = ProductNamesResponseData.FromServiceData(data);
    }

    [TestMethod]
    public void ProductNamesResponseDataEmpty()
    {
      Assert.AreEqual(string.Empty, ProductNamesResponseData.Empty.Name);
      Assert.AreEqual(string.Empty, ProductNamesResponseData.Empty.FriendlyName);
    }
  

    private const int _REQUESTTYPE = 724;

    [TestMethod]
    public void ProductNamesRequestBasic()
    {
      var request = new ProductNamesRequestData("en-us", 5601);
      var response = (ProductNamesResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(string.Empty, response.Name);
      Assert.AreNotEqual(string.Empty, response.FriendlyName);
    }

    [TestMethod]
    public void ProductNamesRequestBasicPT()
    {
      var request = new ProductNamesRequestData("pt-br", 101);
      var response = (ProductNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(string.Empty, response.Name);
      Assert.AreNotEqual(string.Empty, response.FriendlyName);
    }

    [TestMethod]
    public void ProductNamesRequestEmptyLanguage()
    {
      var request = new ProductNamesRequestData(null, 5601);
      var response = (ProductNamesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(ProductNamesResponseData.Empty, response));
      Assert.AreEqual(string.Empty, response.Name);
      Assert.AreEqual(string.Empty, response.FriendlyName);
    }


  }
}
