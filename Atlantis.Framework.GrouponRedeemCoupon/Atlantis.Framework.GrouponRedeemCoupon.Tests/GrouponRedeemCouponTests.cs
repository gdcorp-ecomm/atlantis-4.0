using System;
using Atlantis.Framework.GrouponRedeemCoupon.Interface;
using Atlantis.Framework.GrouponRedeemCoupon.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GrouponRedeemCoupon.Tests
{
  [TestClass]
  public class GrouponRedeemCouponTests
  {
    private int _requestType = 368;
  
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod1()
    {
      var requesData = new GrouponRedeemCouponRequestData("855302", string.Empty, string.Empty, string.Empty, 0,
                                                          "82541-18496-24336-38312");
      var response = (GrouponRedeemCouponResponseData) Engine.Engine.ProcessRequest(requesData, _requestType);
      Assert.IsTrue(response.Status == 0);
    }
  }
}
