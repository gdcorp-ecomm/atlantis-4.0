using System;
using Atlantis.Framework.CostcoGetMemberInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CostcoGetMemberInfo.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CostcoGetMemberInfoTests
  {

    private int _requestType = 368;

    [TestMethod]
    public void TestCostcoGetMemberInfoRequest_ValidShopperCostcoMember()
    {
      var requestdata = new CostcoGetMemberInfoRequestData("855302", string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoGetMemberInfoResponseData)Engine.Engine.ProcessRequest(requestdata, _requestType);
      Assert.IsTrue(response.MemberLevelId == 1);
    }

    [TestMethod]
    public void TestCostcoGetMemberInfoRequest_ValidShopperNotMember()
    {
      var requestdata = new CostcoGetMemberInfoRequestData("855552", string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoGetMemberInfoResponseData)Engine.Engine.ProcessRequest(requestdata, _requestType);
      Assert.IsTrue(response.MemberLevelId == 0);
    }

    [TestMethod]
    public void TestCostcoGetMemberInfoRequest_InvalidShopper()
    {
      // service does not validate shopper input.
      var requestdata = new CostcoGetMemberInfoRequestData("blah", string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoGetMemberInfoResponseData)Engine.Engine.ProcessRequest(requestdata, _requestType);
      Assert.IsTrue(response.MemberLevelId == 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestCostcoGetMemberInfoRequest_NullShopperId()
    {
      var requestdata = new CostcoGetMemberInfoRequestData(null, string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoGetMemberInfoResponseData)Engine.Engine.ProcessRequest(requestdata, _requestType);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestCostcoGetMemberInfoRequest_EmptyShopperId()
    {
      var requestdata = new CostcoGetMemberInfoRequestData("", string.Empty, string.Empty, string.Empty, 0);
      var response = (CostcoGetMemberInfoResponseData)Engine.Engine.ProcessRequest(requestdata, _requestType);
    }

  }
}
