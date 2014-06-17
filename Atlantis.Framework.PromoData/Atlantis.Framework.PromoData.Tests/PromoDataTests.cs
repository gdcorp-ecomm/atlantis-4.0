using System;
using Atlantis.Framework.PromoData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PromoData.Tests
{
  [TestClass]
  public class PromoDataTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PromoData.Impl.dll")]
    public void GetPromoData()
    {
      int amount = 0;
      string awardType = string.Empty;
      //PromoDataRequestData request = new PromoDataRequestData("77311", string.Empty,
      //  String.Empty, string.Empty, 0, "sample");
      //PromoDataRequestData request = new PromoDataRequestData("77311", string.Empty,
      //  String.Empty, string.Empty, 0, "iscSSLStd1299");
      PromoDataRequestData request = new PromoDataRequestData("77311", string.Empty,
       String.Empty, string.Empty, 0, "MNIJRFAILMW");
      request.RequestTimeout = new TimeSpan(0,0,60);
      PromoDataResponseData response
        = (PromoDataResponseData)Engine.Engine.ProcessRequest(request, 365);

      if (response.IsValid 
        && response.PromoProduct.IsActivePromoForPrivateLabelTypeId(1))
      {
        IProductAward award = response.PromoProduct.GetProductAward(1);
        amount = award.GetAwardAmount("EUR");
        awardType = award.AwardType;
      }

      //Assert.IsTrue(amount > 0 && !string.IsNullOrEmpty(awardType));
      Assert.IsTrue(response.IsValid);
    }
  }
}
