using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.PromoOrderLevelAddPL.Interface;



namespace Atlantis.Framework.PromoOrderLevelAddPL.Test
{
  [TestClass]
  public class PromoOrderLevelAddPLTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CreatePLOrderLevelPromoSingleReseller_Success()
    {
      PromoOrderLevelAddPLRequestData request = new PromoOrderLevelAddPLRequestData(
        string.Empty, string.Empty, 0, "50Neaz200", "192.168.0.2");

      PrivateLabelPromo promo = new PrivateLabelPromo(440811, "07/01/2012", "07/30/2012", true);
      PrivateLabelPromo promo2 = new PrivateLabelPromo(440812, "07/01/2012", "07/30/2012", true);
      request.AddResellerToPromoList(promo);
      request.AddResellerToPromoList(promo2);

      PromoOrderLevelAddPLResponseData response = Engine.Engine.ProcessRequest(request, 570) as PromoOrderLevelAddPLResponseData;
      Assert.IsTrue(response.IsSuccess() == RequestSuccessCode.Successful);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CreatePLOrderLevelPromoSingleReseller_FailedPromoDoesntExist()
    {
      PromoOrderLevelAddPLRequestData request = new PromoOrderLevelAddPLRequestData(
        string.Empty, string.Empty, 0, "SaXcAr4", "192.168.0.2");

      PrivateLabelPromo promo = new PrivateLabelPromo(440809, "07/01/2012", "07/30/2012", true);
      PrivateLabelPromo promo2 = new PrivateLabelPromo(440810, "07/01/2012", "07/30/2012", true);
      request.AddResellerToPromoList(promo);
      request.AddResellerToPromoList(promo2);

      PromoOrderLevelAddPLResponseData response = Engine.Engine.ProcessRequest(request, 570) as PromoOrderLevelAddPLResponseData;
      Assert.IsTrue(response.IsSuccess() == RequestSuccessCode.FailedPromoCodeDoesNotExist);
    }
  }
}
