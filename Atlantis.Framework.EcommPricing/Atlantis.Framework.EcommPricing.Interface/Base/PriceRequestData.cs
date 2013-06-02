using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface.Base
{
  public abstract class PriceRequestData : RequestData
  {
    public const int DEFAULT_PRICE_GROUP = 0;
    protected static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(5);

    public int UnifiedProductId { get; set; }
    public int PrivateLabelId { get; set; }
    public int ShopperPriceType { get; set; }
    public string CurrencyType { get; set; }
    public int PriceGroupId { get; set; }

    protected PriceRequestData(int unifiedProductId, int privateLabelId, int shopperPriceType, string currencyType, int priceGroupId = DEFAULT_PRICE_GROUP)      
    {
      RequestTimeout = _requestTimeout;

      PrivateLabelId = privateLabelId;
      ShopperPriceType = shopperPriceType;
      UnifiedProductId = unifiedProductId;
      CurrencyType = currencyType;
      PriceGroupId = priceGroupId;
    }

    public virtual string Options
    {
      get { return String.Join(",", ShopperPriceType.ToString(), CurrencyType, PriceGroupId.ToString()); }
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(UnifiedProductId.ToString(), PrivateLabelId.ToString(), Options);
    }

    public override string ToXML()
    {
      var element = new XElement(this.GetType().Name);
      element.Add(new XAttribute("UnifiedProductId", UnifiedProductId.ToString()));
      element.Add(new XAttribute("PrivateLabelId", PrivateLabelId.ToString()));
      element.Add(new XAttribute("ShopperPriceType", ShopperPriceType.ToString()));
      element.Add(new XAttribute("CurrencyType", CurrencyType));
      element.Add(new XAttribute("PriceGroupId", PriceGroupId.ToString()));
      return element.ToString();
    }
  }
}
