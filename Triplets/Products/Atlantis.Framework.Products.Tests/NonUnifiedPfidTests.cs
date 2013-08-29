using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class NonUnifiedPfidTests
  {
    [TestMethod]
    public void NonUnifiedPfidRequestProperties()
    {
      NonUnifiedPfidRequestData request = new NonUnifiedPfidRequestData(101, 1);
      Assert.AreEqual(1, request.PrivateLabelId);
      Assert.AreEqual(101, request.UnifiedProductId);
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void NonUnifiedPfidRequestCacheKeySame()
    {
      NonUnifiedPfidRequestData request1 = new NonUnifiedPfidRequestData(101, 1);
      NonUnifiedPfidRequestData request2 = new NonUnifiedPfidRequestData(101, 1);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void NonUnifiedPfidRequestCacheKeyDifferent()
    {
      NonUnifiedPfidRequestData request1 = new NonUnifiedPfidRequestData(101, 1);
      NonUnifiedPfidRequestData request2 = new NonUnifiedPfidRequestData(101, 2);
      NonUnifiedPfidRequestData request3 = new NonUnifiedPfidRequestData(102, 1);

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request1.GetCacheMD5(), request3.GetCacheMD5());
      Assert.AreNotEqual(request2.GetCacheMD5(), request3.GetCacheMD5());
    }

    [TestMethod]
    public void NonUnifiedPfidResponseDataCreate()
    {
      NonUnifiedPfidResponseData response = NonUnifiedPfidResponseData.FromNonUnifiedPfid(22021);
      Assert.AreEqual(22021, response.NonUnifiedPfid);
      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void NonUnifiedPfidResponseDataCreateNotFound()
    {
      NonUnifiedPfidResponseData response = NonUnifiedPfidResponseData.FromNonUnifiedPfid(0);
      Assert.IsTrue(ReferenceEquals(response, NonUnifiedPfidResponseData.NotFound));
    }

    const int _REQUESTTYPE = 699;

    [TestMethod]
    public void NonUnifiedPfidBlueRazor()
    {
      var request = new NonUnifiedPfidRequestData(101, 2);
      var response = (NonUnifiedPfidResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(101, response.NonUnifiedPfid);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void BadRequest()
    {
      var request = new BadRequestType();
      var response = (NonUnifiedPfidResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

    private class BadRequestType : RequestData
    {
    }

  }
}
