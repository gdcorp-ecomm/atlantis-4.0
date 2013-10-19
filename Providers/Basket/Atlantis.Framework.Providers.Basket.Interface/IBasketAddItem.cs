using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Basket Item
  /// </summary>
  public interface IBasketAddItem
  {
    /// <summary>
    /// Unified product id. Do not use non-unified product ids in this field.
    /// </summary>
    int UnifiedProductId { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    int Quantity { get; set; }

    /// <summary>
    /// Item tracking code
    /// </summary>
    string ItemTrackingCode { get; set; }

    /// <summary>
    /// Pathway (from traffic system)
    /// </summary>
    string Pathway { get; set; }

    /// <summary>
    /// CI Code
    /// </summary>
    string CiCode { get; set; }

    /// <summary>
    /// Overrides the price in the basket with the given price.
    /// </summary>
    /// <param name="overrideListPriceUSD">List price in USD for the override</param>
    /// <param name="overrideCurrentPriceUSD">Current price in USD for the override</param>
    void SetOverridePrice(int overrideListPriceUSD, int overrideCurrentPriceUSD);

    /// <summary>
    /// Gets the override price
    /// </summary>
    IBasketAddPriceOverride OverridePrice { get; }

    /// <summary>
    /// Returns true if an Override Price exists
    /// </summary>
    bool HasOverridePrice { get; }

    /// <summary>
    /// Duration
    /// </summary>
    float? Duration { get; set; }

    /// <summary>
    /// Overidden product name
    /// </summary>
    string OverrideProductName { get; set; }

    /// <summary>
    /// Dicsount Code
    /// </summary>
    string DiscountCode { get; set; }

    /// <summary>
    /// Promo Tracking Code
    /// </summary>
    string PromoTrackingCode { get; set; }

    /// <summary>
    /// Fastball offer id
    /// </summary>
    string FastballOfferId { get; set; }

    /// <summary>
    /// FastballOfferUid
    /// </summary>
    string FastballOfferUid { get; set; }

    /// <summary>
    /// Fastball Discount
    /// </summary>
    string FastballDiscount { get; set; }

    /// <summary>
    /// Resource Id used for renewals
    /// </summary>
    string ResourceId { get; set; }

    /// <summary>
    /// Affiliate data string. 
    /// </summary>
    string AffiliateData { get; set; }

    /// <summary>
    /// Stack id
    /// </summary>
    int? StackId { get; set; }

    /// <summary>
    /// Partner Id
    /// </summary>
    int? PartnerId { get; set; }

    /// <summary>
    /// Redemption code
    /// </summary>
    string RedemptionCode { get; set; }

    /// <summary>
    /// Add a child item to this item. Child items are not allowed to have child items. The basket only allows
    /// one child deep in its structure.
    /// </summary>
    /// <param name="childItem">IBasketItem to add as a child.</param>
    void AddChildItem(IBasketAddItem childItem);

    /// <summary>
    /// Child items
    /// </summary>
    IEnumerable<IBasketAddItem> ChildItems { get; }

    /// <summary>
    /// returns true if the item has child items.
    /// </summary>
    bool HasChildItems { get; }

    /// <summary>
    /// Custom Xml Element to add to the item
    /// </summary>
    XElement CustomXml { get; set; }
  }
}
