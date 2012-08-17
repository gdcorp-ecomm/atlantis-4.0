using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;
using Atlantis.Framework.PromoOrderLevelUpdate.Interface;
using Atlantis.Framework.PromoOrderLevelUpdate.Impl;

namespace Atlantis.Framework.PromoOrderLevelUpdate.Test
{
  [TestClass]
  public class UpdateOrderPromoPLTest
  {

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void UpdateOrderPromoPL_Success()
    {
      OrderLevelPromoVersion promo = new OrderLevelPromoVersion("35NERZ01", "8/1/2012", "8/30/2012", true, "35NERZ12", "35% off for nerdz");

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelUpdateRequestData request = new PromoOrderLevelUpdateRequestData(string.Empty, string.Empty, 0, promo);
      PromoOrderLevelUpdateResponseData response = (PromoOrderLevelUpdateResponseData)Engine.Engine.ProcessRequest(request, 580);

      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelUpdateResponseData.RequestSuccessCode.Success);

    }

  }
}
