using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
    public void OfferedTldsBasic()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);
      Assert.IsTrue(response.OfferedTLDs.Count() > 0);
    }

    [TestMethod]
    public void OfferedTldsIsOffered()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);

      string tld = response.OfferedTLDs.First();
      Assert.IsTrue(response.IsTLDOffered(tld));
    }

    [TestMethod]
    public void OfferedTldsNotOffered()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);

      Assert.IsFalse(response.IsTLDOffered("notavalidtldever"));
    }

    [TestMethod]
    public void OfferedTldsIsOfferedNull()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);

      Assert.IsFalse(response.IsTLDOffered(null));
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
    public void OfferedTldSortOrder()
    {
      var request = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, OfferedTLDProductTypes.Registration);
      var response = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _OFFEREDTLDREQUEST);

      var sortOrder = response.GetSortOrder();
      int itemCount = response.OfferedTLDs.Count();

      Assert.AreEqual(itemCount, sortOrder.Count);
    }



  }
}
