using Atlantis.Framework.Interface;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class PriceEstimateRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(5);

    private static readonly Regex _validCharsEx = new Regex("^[0-9A-Z]*$", RegexOptions.Singleline | RegexOptions.Compiled);
    const int _MAXLENGTH = 20;

    public int PrivateLabelId { get; set; }
    public int ShopperPriceType { get; set; }
    public string CurrencyType { get; set; }
    public string PromoCode { get; set; }
    public string DiscountCode { get; set; }
    public int PriceGroupId { get; set; }
    public int UnifiedProductId { get; set; }

    public PriceEstimateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, int shopperPriceType, string currencyType, string promoCode, int priceGroupId, int unifiedProductId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      ShopperPriceType = shopperPriceType;
      CurrencyType = currencyType;
      PromoCode = PreValidatePromoCode(promoCode);
      DiscountCode = string.Empty;
      PriceGroupId = priceGroupId;
      UnifiedProductId = unifiedProductId;
      RequestTimeout = _requestTimeout;
    }

    public PriceEstimateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, int shopperPriceType, string currencyType, string promoCode, int priceGroupId, int unifiedProductId, string discountCode)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      ShopperPriceType = shopperPriceType;
      CurrencyType = currencyType;
      PromoCode = PreValidatePromoCode(promoCode);
      DiscountCode = discountCode;
      PriceGroupId = priceGroupId;
      UnifiedProductId = unifiedProductId;
      RequestTimeout = _requestTimeout;
    }

    private string PreValidatePromoCode(string promoCode)
    {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(promoCode))
      {
        promoCode = promoCode.ToUpperInvariant();
        if (_validCharsEx.IsMatch(promoCode))
        {
          result = promoCode;
          if (result.Length > _MAXLENGTH)
          {
            result = result.Substring(0, _MAXLENGTH);
          }
        }
      }
      return result;
    }

    public override string GetCacheMD5()
    {
      var values = new[] { PrivateLabelId.ToString(), ShopperPriceType.ToString(), CurrencyType, PromoCode, PriceGroupId.ToString(), UnifiedProductId.ToString(), DiscountCode };
      return BuildHashFromStrings(values);
    }

    public override string ToXML()
    {
      var element = new XElement("PriceEstimateRequestData");
      element.Add(new XAttribute("PrivateLabelId", PrivateLabelId.ToString()));
      element.Add(new XAttribute("ShopperPriceType", ShopperPriceType.ToString()));
      element.Add(new XAttribute("CurrencyType", CurrencyType));
      element.Add(new XAttribute("PromoCode", PromoCode));
      element.Add(new XAttribute("PriceGroupId", PriceGroupId.ToString()));
      element.Add(new XAttribute("UnifiedProductId", UnifiedProductId.ToString()));
      element.Add(new XAttribute("DiscountCode", DiscountCode));
      return element.ToString();
    }
  }
}
