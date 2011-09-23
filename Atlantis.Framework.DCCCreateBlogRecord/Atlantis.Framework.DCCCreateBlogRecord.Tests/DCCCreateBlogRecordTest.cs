using System;
using Atlantis.Framework.DCCCreateBlogRecord.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCCreateBlogRecord.Tests
{
  [TestClass]
  public class DCCCreateBlogRecordTest
  {
    private int _requestType = 356;

    private string shopperId = "855552";
    private string ownedDomain = "ADASDASD.CO";
    private string notownedDomain = "dontown.org";
    private string subDomain = "blog";
    private string subDomainLong = "my.blog";


    [TestMethod]
    public void CreateBlogRecord_ShopperOwnsDomain()
    {
      var request = new DCCCreateBlogRecordRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, string.Empty, ownedDomain, "googleblog", "123.123.123.123");
      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DCCCreateBlogRecordResponseData response = (DCCCreateBlogRecordResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void CreateBlogRecord_ShopperOwnsDomain_Subdomain()
    {
      var request = new DCCCreateBlogRecordRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, subDomain, ownedDomain, "googleblog", "123.123.123.123");
      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DCCCreateBlogRecordResponseData response = (DCCCreateBlogRecordResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void CreateBlogRecord_ShopperOwnsDomain_SubdomainLong()
    {
      var request = new DCCCreateBlogRecordRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, subDomainLong, ownedDomain, "googleblog", "123.123.123.123");
      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DCCCreateBlogRecordResponseData response = (DCCCreateBlogRecordResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void CreateBlogRecord_ShopperDoesNotOwnDomain()
    {
      var request = new DCCCreateBlogRecordRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, string.Empty, notownedDomain, "googleblog", "123.123.123.123");
      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DCCCreateBlogRecordResponseData response = (DCCCreateBlogRecordResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.ErrorNum == 115 || response.ErrorNum == 500);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void CreateBlogRecord_BadRequest()
    {
      var request = new DCCCreateBlogRecordRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, string.Empty, notownedDomain, "googleblog-test-wontwork", "123.123.123.123");
      request.RequestTimeout = TimeSpan.FromSeconds(30);
      DCCCreateBlogRecordResponseData response = (DCCCreateBlogRecordResponseData)Engine.Engine.ProcessRequest(request, _requestType);
    }
  }
}
