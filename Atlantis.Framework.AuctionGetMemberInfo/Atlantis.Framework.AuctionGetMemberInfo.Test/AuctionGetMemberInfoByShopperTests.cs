using System;
using System.Diagnostics;
using Atlantis.Framework.AuctionGetMemberInfo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionGetMemberInfo.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class AuctionGetMemberInfoTests
  {

    protected const string shopperId = "859775"; //"847331";// //"847331";
    protected const int requestId = 380;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMemberInfoByShopperId()
    {

      AuctionGetMemberInfoResponseData response = null;

      var request = new AuctionGetMemberInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0);
      request.RequestTimeout = new TimeSpan(0, 0, 0, 30);

      response = (AuctionGetMemberInfoResponseData)Engine.Engine.ProcessRequest(request, requestId);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.Member);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine("HasAdultFilter:" + response.Member.HasAdultFilter);
      Debug.WriteLine("IsGoodAsGoldEnabled:" + response.Member.IsGoodAsGoldEnabled);
      Debug.WriteLine("MemberId:" + response.Member.MemberId);
      Debug.WriteLine("PrivateLabelId:" + response.Member.PrivateLabelId);
      Debug.WriteLine("ShopperId:" + response.Member.ShopperId);
      Debug.WriteLine("StatusCodeId:" + response.Member.StatusCodeId);
      Debug.WriteLine("SubscriptionActive:" + response.Member.SubscriptionActive);


    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMemberInfoByShopperIdUsingSessionCache()
    {
      MockHttpContext.SetMockHttpContext("test.aspx", "http://127.0.0.1/test.aspx", string.Empty);

      AuctionGetMemberInfoResponseData response1 = null;
      AuctionGetMemberInfoResponseData response2 = null;

      var request = new AuctionGetMemberInfoRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0);
      request.RequestTimeout = new TimeSpan(0, 0, 0, 30);

      response1 = (AuctionGetMemberInfoResponseData)SessionCache.SessionCache.GetProcessRequest<AuctionGetMemberInfoResponseData>(request, requestId);

      Assert.IsTrue(response1.IsSuccess);
      Assert.IsNotNull(response1.Member);

      response2 = (AuctionGetMemberInfoResponseData)SessionCache.SessionCache.GetProcessRequest<AuctionGetMemberInfoResponseData>(request, requestId);
      Assert.IsTrue(response2.IsSuccess);
      Assert.IsNotNull(response2.Member);

      if (response1.Member.HasAdultFilter)
      {
      }

      Assert.AreEqual(response1.Member.HasAdultFilter, response2.Member.HasAdultFilter);
      Assert.AreEqual(response1.Member.IsGoodAsGoldEnabled, response2.Member.IsGoodAsGoldEnabled);
      Assert.AreEqual(response1.Member.MemberId, response2.Member.MemberId);
      Assert.AreEqual(response1.Member.PrivateLabelId, response2.Member.PrivateLabelId);
      Assert.AreEqual(response1.Member.ShopperId, response2.Member.ShopperId);
      Assert.AreEqual(response1.Member.StatusCodeId, response2.Member.StatusCodeId);
    }
  }
}
