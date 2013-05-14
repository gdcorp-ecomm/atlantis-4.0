using Atlantis.Framework.DataCacheGeneric.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.DataCacheGeneric.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class GetCacheDataTests
  {
    [TestMethod]
    public void GetCacheDataRequestDataCacheKey()
    {
      string requestXml = "<LinkInfo><param name=\"contextID\" value=\"1\" /></LinkInfo>";
      GetCacheDataRequestData request1 = new GetCacheDataRequestData(requestXml);
      GetCacheDataRequestData request1same = new GetCacheDataRequestData(requestXml);
      Assert.AreEqual(request1.GetCacheMD5(), request1same.GetCacheMD5());

      string requestXml2 = "<GetCountryListByRegion><param name=\"regionId\" value = \"9\" /></GetCountryListByRegion>";
      GetCacheDataRequestData request2 = new GetCacheDataRequestData(requestXml2);
      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void GetCacheDataRequestDataToXml()
    {
      string requestXml = "<LinkInfo><param name=\"contextID\" value=\"1\" /></LinkInfo>";
      GetCacheDataRequestData request1 = new GetCacheDataRequestData(requestXml);
      Assert.AreEqual(requestXml, request1.ToXML());
    }

    [TestMethod]
    public void GetCacheDataResponseDataProperties()
    {
      string responseXml = "<data count=\"0\" />";
      GetCacheDataResponseData response = GetCacheDataResponseData.FromCacheDataXml(responseXml);
      Assert.AreEqual(responseXml, response.ToXML());
      Assert.IsNull(response.GetException());
    }

    const int REQUESTTYPE = 694;

    [TestMethod]
    public void GetCacheDataRequest()
    {
      string requestXml = "<LinkInfo><param name=\"contextID\" value=\"1\" /></LinkInfo>";
      GetCacheDataRequestData request = new GetCacheDataRequestData(requestXml);
      GetCacheDataResponseData response = (GetCacheDataResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      XElement.Parse(response.ToXML());
    }
  }
}
