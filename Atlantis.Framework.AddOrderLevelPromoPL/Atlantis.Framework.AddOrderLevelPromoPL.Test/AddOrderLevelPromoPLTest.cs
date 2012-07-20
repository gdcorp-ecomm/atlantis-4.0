using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Impl;

namespace Atlantis.Framework.AddOrderLevelPromoPL.Test
{
  [TestClass]
  public class AddOrderLevelPromoPLTest
  {

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_Success()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERZ01", "08/01/2012", "08/30/2012", true, "35NERZ01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.Success);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_ExpiredEndDate()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEAL01","07/01/2012", "07/31/2011", true, "35NEAL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.FailedInvalidDateSpecification);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoCodeInUse()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEAL01", "07/01/2012", "07/31/2012", true, "35NEAL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.FailedPromoAlreadyExists);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoPctAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEQL01", "07/01/2012", "07/31/2012", true, "35NEQL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoDollarAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEQL01", "07/01/2012", "07/31/2012", true, "35NEQL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.DollarOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoInvalidCurrency()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEQL01", "07/01/2012", "07/31/2012", true, "35NEQL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.DollarOff, "USQ", 10000);
      promo.Currencies.Add(currency);

      PLOrderLevelPromoRequestData request = new PLOrderLevelPromoRequestData(string.Empty, string.Empty, 0, promo);

      PLOrderLevelPromoResponseData response = (PLOrderLevelPromoResponseData)Engine.Engine.ProcessRequest(request, 778);
      Assert.IsTrue(response.IsSuccess() == PLOrderLevelPromoResponseData.RequestSuccessCode.FailedInvalidCurrencySpecification);
    }
  }
}
