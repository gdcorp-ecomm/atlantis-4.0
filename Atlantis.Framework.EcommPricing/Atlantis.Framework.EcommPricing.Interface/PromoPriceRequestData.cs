using System.Xml.Linq;
using Atlantis.Framework.EcommPricing.Interface.Base;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class PromoPriceRequestData : PriceRequestData
  {
    public int Quantity { get; set; }

    public PromoPriceRequestData(int unifiedProductId, int privateLabelId, int quantity, int shopperPriceType, string currencyType, int priceGroupId = DEFAULT_PRICE_GROUP) :
      base(unifiedProductId, privateLabelId, shopperPriceType, currencyType, priceGroupId)
    {
      Quantity = quantity;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(UnifiedProductId.ToString(), PrivateLabelId.ToString(), Quantity.ToString(), ShopperPriceType.ToString(), CurrencyType, PriceGroupId.ToString());
    }

    public override string ToXML()
    {
      string baseXml = base.ToXML();
      XElement element = XElement.Parse(baseXml);
      element.Add(new XAttribute("Quantity", Quantity.ToString()));
      return element.ToString();
    }
  }
}
