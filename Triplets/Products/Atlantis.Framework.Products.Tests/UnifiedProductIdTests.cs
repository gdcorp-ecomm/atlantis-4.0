using Atlantis.Framework.Products.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class UnifiedProductIdTests
  {
    [TestMethod]
    public void RequestDataProperties()
    {
      UnifiedProductIdRequestData request = new UnifiedProductIdRequestData(22021, 2);
      Assert.AreEqual(2, request.PrivateLabelId);
      Assert.AreEqual(22021, request.NonUnifiedPfid);
      Assert.IsFalse(string.IsNullOrEmpty(request.GetCacheMD5()));
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferent()
    {
      UnifiedProductIdRequestData request = new UnifiedProductIdRequestData(22021, 2);
      UnifiedProductIdRequestData request2 = new UnifiedProductIdRequestData(22021, 1724);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());

      request = new UnifiedProductIdRequestData(22021, 2);
      request2 = new UnifiedProductIdRequestData(22022, 2);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());

      request = new UnifiedProductIdRequestData(22021, 2);
      request2 = new UnifiedProductIdRequestData(22022, 1724);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeySame()
    {
      UnifiedProductIdRequestData request = new UnifiedProductIdRequestData(2500101, 2);
      UnifiedProductIdRequestData request2 = new UnifiedProductIdRequestData(2500101, 2);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ResponseDataProperties()
    {
      string cacheData = "<data><item gdshop_product_unifiedProductID=\"3\" /></data>";
      var response = UnifiedProductIdResponseData.FromCacheData(cacheData);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(3, response.UnifiedProductId);
      XElement.Parse(response.ToXML());
      Assert.IsFalse(ReferenceEquals(response, UnifiedProductIdResponseData.NotFound));
    }

    [TestMethod]
    public void ResponseDataNotFoundBadXml()
    {
      string cacheData = "<data><item gdshop_product_unifiedProductID=\"3\" />";
      var response = UnifiedProductIdResponseData.FromCacheData(cacheData);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(int.MinValue, response.UnifiedProductId);
      Assert.IsTrue(ReferenceEquals(response, UnifiedProductIdResponseData.NotFound));
    }

    [TestMethod]
    public void ResponseDataNotFoundMissingAttribute()
    {
      string cacheData = "<data><item /></data>";
      var response = UnifiedProductIdResponseData.FromCacheData(cacheData);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(int.MinValue, response.UnifiedProductId);
      Assert.IsTrue(ReferenceEquals(response, UnifiedProductIdResponseData.NotFound));
    }

    [TestMethod]
    public void ResponseDataNotFoundBadAttribute()
    {
      string cacheData = "<data><item gdshop_product_unifiedProductID=\"B3\" /></data>";
      var response = UnifiedProductIdResponseData.FromCacheData(cacheData);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(int.MinValue, response.UnifiedProductId);
      Assert.IsTrue(ReferenceEquals(response, UnifiedProductIdResponseData.NotFound));
    }

    const int _REQUESTTYPE = 700;

    [TestMethod]
    public void RequestBasic()
    {
      var request = new UnifiedProductIdRequestData(2500101, 2);
      var response = (UnifiedProductIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(101, response.UnifiedProductId);
    }

    [TestMethod]
    public void RequestException()
    {
      var request = new NonUnifiedPfidRequestData(2500101, 2); // using wrong type causes exception
      var response = (UnifiedProductIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(UnifiedProductIdResponseData.NotFound, response));
    }

  }
}
