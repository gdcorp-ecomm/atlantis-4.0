using System;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.EcommPricing.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommPricing.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PriceGroups.Impl.dll")]  
  public class PriceGroupsByCountrySiteTests
  {
    const int _REQUESTTYPE = 713;

    [TestMethod]
    public void PriceGroupsByCountrySiteRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new PriceGroupsByCountrySiteRequestData();
      Assert.IsNotNull(request);
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteRequestDataCacheKeySame()
    {
      var request1 = new PriceGroupsByCountrySiteRequestData();
      var request2 = new PriceGroupsByCountrySiteRequestData();
      Assert.AreNotEqual(request1, request2);
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5()); 
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataNullMappingReturnsNoPriceGroupMappingsResponse()
    {
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(null);
      Assert.AreSame(PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse, response);
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataWhitespaceMappingReturnsNoPriceGroupMappingsResponse()
    {
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(String.Empty);
      Assert.AreEqual(PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse, response);

      response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping("  ");
      Assert.AreSame(PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse, response);
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataReturnsLastPriceGroupForSameCountrySite()
    {
      string mapping = "US:100|US:1|US:99";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.AreEqual(99, response.GetPriceGroupId("US"));
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataNotAffectedByInvalidMappings()
    {
      string mapping = "|US:1||UK:|:99|PH:2|IN:BAD|";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.AreEqual(1, response.GetPriceGroupId("US"));
      Assert.AreEqual(2, response.GetPriceGroupId("PH"));
      Assert.AreEqual(0, response.GetPriceGroupId("UK"));
      Assert.AreEqual(0, response.GetPriceGroupId("IN"));
      Assert.AreEqual(0, response.GetPriceGroupId(null));
      Assert.AreEqual(0, response.GetPriceGroupId(String.Empty));
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataGetPriceGroupNotCaseSensitive()
    {
      string mapping = "US:1";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.AreEqual(1, response.GetPriceGroupId("US"));
      Assert.AreEqual(1, response.GetPriceGroupId("us"));
      Assert.AreEqual(1, response.GetPriceGroupId("Us"));
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataGetPriceGroupForUnmappedCountrySiteReturnsZero()
    {
      string mapping = "US:1";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.AreEqual(0, response.GetPriceGroupId("PH"));      
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataGetExceptionReturnsNull()
    {
      string mapping = "US:1";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteResponseDataToXMLReturnsEmptyString()
    {
      string mapping = "US:1";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      Assert.AreEqual(String.Empty, response.ToXML());
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteRequestReturnsNoPriceGroupMappingsResponseIfPassedInvalidRequestData()
    {
      var request = new FakeRequestData();
      var response = (PriceGroupsByCountrySiteResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreSame(PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse, response);
    }

    [TestMethod]
    public void PriceGroupsByCountrySiteRequestReturnsValidResponse()
    {
      var request = new PriceGroupsByCountrySiteRequestData();
      var response = (PriceGroupsByCountrySiteResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsNotNull(response);
    }
  }
}
