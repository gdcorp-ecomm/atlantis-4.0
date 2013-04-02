﻿using Atlantis.Framework.Geo.Interface;
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
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class RegionTests
  {
    const int _REGIONREQUESTTYPE = 666;

    [TestMethod]
    public void RegionRequestProperties()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "EU");
      Assert.AreEqual(2, request.RegionTypeId);
      Assert.AreEqual("EU", request.RegionName);
    }

    [TestMethod]
    public void RegionRequestCacheKeySame()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "EU");
      RegionRequestData request2 = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "eu");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RegionRequestCacheKeyDifferent()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "ap");
      RegionRequestData request2 = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "eu");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RegionRequestXml()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "EU");
      string xml = request.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void RegionResponseException()
    {
      AtlantisException exception = new AtlantisException("RegionTests.RegionResponseException", "0", "TestMessage", "TestData", null, null);
      RegionResponseData response = RegionResponseData.FromException(exception);
      Assert.IsNotNull(response.GetException());
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void RegionResponseBadXml()
    {
      string cacheXml = "<data count=\"28\"><item CountryId=\"14\"/><";
      RegionResponseData response = RegionResponseData.FromDataCacheXml(cacheXml);
    }

    [TestMethod]
    public void RegionResponseValid()
    {
      string cacheXml = "<data count=\"1\"><item CountryId=\"14\"/></data>";
      RegionResponseData response = RegionResponseData.FromDataCacheXml(cacheXml);
      Assert.AreNotEqual(0, response.Count);
    }

    [TestMethod]
    public void RegionResponseNoData()
    {
      string cacheXml = "<data></data>";
      RegionResponseData response = RegionResponseData.FromDataCacheXml(cacheXml);
      Assert.AreEqual(0, response.Count);
      Assert.AreEqual(RegionResponseData.Empty, response);
    }

    [TestMethod]
    public void GetRegions()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "EU");
      RegionResponseData response = (RegionResponseData)Engine.Engine.ProcessRequest(request, _REGIONREQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
      Assert.AreEqual(response.CountryIds.Count(), response.Count);

      string xml = response.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void GetRegionsNullName()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, null);
      RegionResponseData response = (RegionResponseData)Engine.Engine.ProcessRequest(request, _REGIONREQUESTTYPE);
      Assert.AreEqual(0, response.Count);
      Assert.AreEqual(RegionResponseData.Empty, response);

      string xml = response.ToXML();
      XElement.Parse(xml);
    }


    [TestMethod]
    public void HasCountryId()
    {
      RegionRequestData request = new RegionRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "EU");
      RegionResponseData response = (RegionResponseData)Engine.Engine.ProcessRequest(request, _REGIONREQUESTTYPE);
      Assert.IsTrue(response.HasCountry(14));
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void GetCountriesExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (RegionResponseData)Engine.Engine.ProcessRequest(request, _REGIONREQUESTTYPE);
    }




  }
}