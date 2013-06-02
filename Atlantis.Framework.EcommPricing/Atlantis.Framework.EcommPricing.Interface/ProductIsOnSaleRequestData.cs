using Atlantis.Framework.EcommPricing.Interface.Base;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class ProductIsOnSaleRequestData : PriceRequestData
  {
    public ProductIsOnSaleRequestData(int unifiedProductId, int privateLabelId, int shopperPriceType, string currencyType, int priceGroupId = DEFAULT_PRICE_GROUP)      
      : base(unifiedProductId, privateLabelId, shopperPriceType, currencyType, priceGroupId) { }
  }
}
