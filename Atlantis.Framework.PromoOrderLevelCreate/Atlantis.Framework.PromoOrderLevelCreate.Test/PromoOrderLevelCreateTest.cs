using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Impl;

namespace Atlantis.Framework.PromoOrderLevelCreate.Test
{
  [TestClass]
  public class PromoOrderLevelCreateTest
  {

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_Success()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", "08/01/2012", "08/30/2012", true, "35NERV01", "15% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1500, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.Success);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_ExpiredEndDate()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", "07/01/2011", "07/31/2011", true, "35NERV01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidDateSpecification);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoCodeInUse()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", "07/01/2012", "07/31/2012", true, "35NEAL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedPromoAlreadyExists);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoPctAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERX01", "07/01/2012", "07/31/2012", true, "35NEXL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }


    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoDollarAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERX01", "07/01/2012", "07/31/2012", true, "35NERX01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.DollarOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void CreateOrderLevelPromoPL_PromoInvalidCurrency()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEQL01", "07/01/2012", "07/31/2012", true, "35NEQL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.DollarOff, "USQ", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidCurrencySpecification);
    }
  }
}
