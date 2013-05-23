using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Impl;

namespace Atlantis.Framework.PromoOrderLevelCreate.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PromoOrderLevelCreate.Impl.dll")]
  public class PromoOrderLevelCreateTest
  {
    string _validStart = DateTime.Now.AddMonths(2).ToShortDateString();
    string _validEnd = DateTime.Now.AddMonths(3).ToShortDateString();
    string _pastStart = DateTime.Now.AddMonths(-3).ToShortDateString();
    string _pastEnd = DateTime.Now.AddMonths(-2).ToShortDateString();

    [TestMethod]
    public void CreateOrderLevelPromoPL_Success()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", _validStart, _validEnd, true, "35NERV01", "15% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1500, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.Success);
    }


    [TestMethod]
    public void CreateOrderLevelPromoPL_ExpiredEndDate()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", _pastStart, _pastEnd, true, "35NERV01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidDateSpecification);
    }


    [TestMethod]
    public void CreateOrderLevelPromoPL_PromoCodeInUse()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERV01", _validStart, _validEnd, true, "35NEAL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedPromoAlreadyExists);
    }


    [TestMethod]
    public void CreateOrderLevelPromoPL_PromoPctAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERX01", _validStart, _validEnd, true, "35NEXL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.PercentOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }


    [TestMethod]
    public void CreateOrderLevelPromoPL_PromoDollarAwardGreaterThanAllowed()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NERX01", _validStart, _validEnd, true, "35NERX01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(2500, PrivateLabelPromoCurrency.AwardType.DollarOff, "USD", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidAwardSpecification);
    }

    [TestMethod]
    public void CreateOrderLevelPromoPL_PromoInvalidCurrency()
    {
      OrderLevelPromo promo = new OrderLevelPromo("35NEQL01", _validStart, _validEnd, true, "35NEQL01", "20% off purchase of $100 or more");
      promo.IsActive = true;

      PrivateLabelPromoCurrency currency = new PrivateLabelPromoCurrency(1800, PrivateLabelPromoCurrency.AwardType.DollarOff, "USQ", 10000);
      promo.Currencies.Add(currency);

      PromoOrderLevelCreateRequestData request = new PromoOrderLevelCreateRequestData(string.Empty, string.Empty, 0, promo);

      PromoOrderLevelCreateResponseData response = (PromoOrderLevelCreateResponseData)Engine.Engine.ProcessRequest(request, 569);
      Assert.IsTrue(response.IsSuccess() == PromoOrderLevelCreateResponseData.RequestSuccessCode.FailedInvalidCurrencySpecification);
    }
  }
}
