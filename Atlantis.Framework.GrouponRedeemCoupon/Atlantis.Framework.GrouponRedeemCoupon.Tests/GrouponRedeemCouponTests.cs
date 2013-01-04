using System;
using Atlantis.Framework.GrouponRedeemCoupon.Interface;
using Atlantis.Framework.GrouponRedeemCoupon.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GrouponRedeemCoupon.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.GrouponRedeemCoupon.Impl.dll")]
  public class GrouponRedeemCouponTests
  {
    private int _requestType = 368;
  
    [TestMethod]
    public void CouponAlreadyUsed()
    {
      var requesData = new GrouponRedeemCouponRequestData("855302", string.Empty, string.Empty, string.Empty, 0,
                                                          "82573-66765-23424-43964");
      var response = (GrouponRedeemCouponResponseData) Engine.Engine.ProcessRequest(requesData, _requestType);
      Assert.AreEqual(GrouponRedeemStatus.InvalidCodeShopperAlreadyUsedAnother, response.Status);
    }

  }
}
