using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketAddItem : IBasketAddItem
  {
    private readonly List<IBasketAddItem> _childItems;
    private readonly ISiteContext _siteContext;
    private IBasketAddPriceOverride _priceOverride;

    internal BasketAddItem(ISiteContext siteContext, int unifiedProductId, string itemTrackingCode)
    {
      _siteContext = siteContext;
      _childItems = new List<IBasketAddItem>(4);
      Quantity = 1;
      UnifiedProductId = unifiedProductId;
      ItemTrackingCode = itemTrackingCode;
    }

    public int UnifiedProductId {get; set;}
    public int Quantity {get; set;}
    public string ItemTrackingCode {get; set;}
    public string Pathway {get; set;}
    public string CiCode {get; set;}
    public float? Duration {get; set;}
    public string OverrideProductName { get; set; }
    public string DiscountCode { get; set; }
    public string PromoTrackingCode { get; set; }
    public string FastballOfferId { get; set; }
    public string FastballOfferUid { get; set; }
    public string FastballDiscount { get; set; }
    public string ResourceId { get; set; }
    public string AffiliateData { get; set; }
    public int? StackId { get; set; }
    public int? PartnerId { get; set; }
    public string RedemptionCode { get; set; }

    public void SetOverridePrice(int overrideListPriceUSD, int overrideCurrentPriceUSD)
    {
      _priceOverride = new BasketAddPriceOverride(_siteContext.PrivateLabelId, UnifiedProductId, overrideCurrentPriceUSD, overrideListPriceUSD);
    }

    public IBasketAddPriceOverride OverridePrice
    {
      get { return _priceOverride; }
    }

    public bool HasOverridePrice
    {
      get { return _priceOverride != null; }
    }

    public XElement CustomXml { get; set; }

    public void AddChildItem(IBasketAddItem childItem)
    {
      if (childItem != null)
      {
        _childItems.Add(childItem);
      }
    }

    public IEnumerable<IBasketAddItem> ChildItems
    {
      get { return _childItems; }
    }

    public bool HasChildItems
    {
      get { return _childItems.Count > 0; }
    }

  }
}
