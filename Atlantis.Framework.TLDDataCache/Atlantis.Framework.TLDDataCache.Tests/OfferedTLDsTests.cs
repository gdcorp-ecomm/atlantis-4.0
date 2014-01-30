using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class OfferedTLDsTests
  {
    const int _OFFEREDTLDREQUEST = 637;

    [TestMethod]
    public void OfferedTldsInvalid()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Invalid);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);
      Assert.IsTrue(!response.OfferedTLDs.Any());
    }

    [TestMethod]
    public void OfferedTldsBasic()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);
      Assert.IsTrue(response.OfferedTLDs.Any());
    }

    [TestMethod]
    public void OfferedTldsDifferentTypes()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);

      var requestBulk = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Bulk);
      var responseBulk = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(requestBulk, _OFFEREDTLDREQUEST);

      Assert.AreNotEqual(response.OfferedTLDs.Count(), responseBulk.OfferedTLDs.Count());
    }

    [TestMethod]
    public void OfferedTldsRequestToXml()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var requestXml = request.ToXML();
      var requestXmlElement = XElement.Parse(requestXml);
      Assert.IsTrue(!string.IsNullOrEmpty(requestXml) && requestXmlElement != null);
    }

    [TestMethod]
    public void OfferedTldsRequestGetCacheMd5()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var requestCacheMd5 = request.GetCacheMD5();
      Assert.IsTrue(!string.IsNullOrEmpty(requestCacheMd5));
    }
  }
}
