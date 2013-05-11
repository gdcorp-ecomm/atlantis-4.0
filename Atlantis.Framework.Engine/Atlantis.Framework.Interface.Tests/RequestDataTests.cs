using Atlantis.Framework.Interface.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Interface.Tests
{
  [TestClass]
  public class RequestDataTests
  {
    [TestMethod]
    public void RequestDataSimpleConstructor()
    {
      RequestDataWithoutBaseArgs testRequest = new RequestDataWithoutBaseArgs("woot");
      Assert.AreEqual("woot", testRequest.MyArg);
      Assert.AreEqual(string.Empty, testRequest.ShopperID);
      Assert.AreEqual(string.Empty, testRequest.Pathway);
      Assert.AreEqual(string.Empty, testRequest.OrderID);
      Assert.AreEqual(string.Empty, testRequest.SourceURL);
      Assert.AreEqual(0, testRequest.PageCount);
    }

    [TestMethod]
    public void RequestDataOldConstructor()
    {
      RequestDataWithBaseArgs testRequest = new RequestDataWithBaseArgs();
      Assert.AreEqual("shopperid", testRequest.ShopperID);
      Assert.AreEqual("pathway", testRequest.Pathway);
      Assert.AreEqual("orderid", testRequest.OrderID);
      Assert.AreEqual("sourceurl", testRequest.SourceURL);
      Assert.AreEqual(1, testRequest.PageCount);
    }

    [TestMethod]
    public void RequestDataNotCacheable()
    {
      try
      {
        RequestDataNonCacheable request = new RequestDataNonCacheable();
        string key = request.GetCacheMD5();
      }
      catch (NotImplementedException ex)
      {
        Assert.IsTrue(ex.Message.Contains("RequestDataNonCacheable"));
        return;
      }

      Assert.Fail("Exception not properly thrown.");
    }

    [TestMethod]
    public void RequestDataCacheable()
    {
      RequestDataCacheable request = new RequestDataCacheable();
      Assert.AreEqual("CACHEME", request.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataDefaults()
    {
      RequestDataCacheable request = new RequestDataCacheable();
      Assert.AreEqual(30, (int)request.RequestTimeout.TotalSeconds);

      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void RequestDataSetProperties()
    {
      RequestDataCacheable request = new RequestDataCacheable();
      request.RequestTimeout = TimeSpan.FromSeconds(23);
      Assert.AreEqual(23, (int)request.RequestTimeout.TotalSeconds);

      request.ShopperID = "me";
      Assert.AreEqual("me", request.ShopperID);
    }

    [TestMethod]
    public void RequestDataCacheKeyAndBuildHash()
    {
      RequestDataCacheKey request1 = new RequestDataCacheKey("blue");
      RequestDataCacheKey request2 = new RequestDataCacheKey("blue");
      RequestDataCacheKey request3 = new RequestDataCacheKey("red");

      Assert.IsFalse(request1.Equals(request2));
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request1.GetCacheMD5(), request3.GetCacheMD5());
    }
  }
}
