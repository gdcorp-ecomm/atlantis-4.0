using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;
using Atlantis.Framework.UpdateOrderPromoPL.Interface;
using Atlantis.Framework.UpdateOrderPromoPL.Impl;

namespace Atlantis.Framework.UpdateOrderPromoPL.Test
{
  [TestClass]
  public class UpdateOrderPromoPLTest
  {

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void UpdateOrderPromoPL_Success()
    {
      OrderLevelPromoVersion promo = new OrderLevelPromoVersion("35NERZ01", 2, "8/1/2012", "8/30/2012", true, "35NERZ12", "35% off for nerdz");

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      UpdateOrderPromoPLRequestData request = new UpdateOrderPromoPLRequestData(string.Empty, string.Empty, 0, promo);
      UpdateOrderPromoPLResponseData response = (UpdateOrderPromoPLResponseData)Engine.Engine.ProcessRequest(request, 779);

      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.Success);

    }

  }
}
