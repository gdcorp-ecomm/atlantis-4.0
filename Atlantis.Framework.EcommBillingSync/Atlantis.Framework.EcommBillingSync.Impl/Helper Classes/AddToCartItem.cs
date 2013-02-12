using System.Collections.Generic;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class AddToCartItem : Dictionary<string, string>
  {
    private string _customXml = string.Empty;

    public AddToCartItem(string formId, int unifiedProductId, int billingResourceId, int quantity, string itemTrackingCode, string duration, string durationHash)
    {
      UnifiedProductId = unifiedProductId;
      BillingResourceId = billingResourceId;
      this[AddToCartItemProperty.FORM_ID] = formId;
      this[AddToCartItemProperty.RESOURCE_ID] = billingResourceId.ToString();
      this[AddToCartItemProperty.UNIFIED_PRODUCT_ID] = unifiedProductId.ToString();
      this[AddToCartItemProperty.QUANTITY] = quantity.ToString();
      this[AddToCartItemProperty.ITEM_TRACKING_CODE] = itemTrackingCode;
      this[AddToCartItemProperty.DURATION] = duration;
      this[AddToCartItemProperty.DURATION_HASH] = durationHash;
    }

    public int BillingResourceId { get; private set; }

    public int UnifiedProductId { get; private set; }

    public string CustomXml
    {
      get { return _customXml; }
      set { _customXml = value; }
    }
  }

  public static class AddToCartItemProperty
  {
    public const string UNIFIED_PRODUCT_ID = "unifiedProductID";
    public const string QUANTITY = "quantity";
    public const string ITEM_TRACKING_CODE = "itemTrackingCode";
    public const string PATHWAY = "pathway";
    public const string FROM_APP = "from_app";
    public const string PARENT_GROUP_ID = "parent_group_id";
    public const string GROUP_ID = "group_id";
    public const string CI_CODE = "ciCode";
    public const string OV_LIST_PRICE = "OV_ListPrice";
    public const string OV_CURRENT_PRICE = "OV_CurrentPrice";
    public const string OV_HASH = "OV_Hash";
    public const string DURATION = "duration";
    public const string DURATION_HASH = "Duration_Hash";
    public const string OV_PRODUCT_NAME = "OV_ProductName";
    public const string DISCOUNT_CODE = "discountCode";
    public const string PROMO_TRACKING_CODE = "promoTrackingCode";
    public const string FASTBALL_OFFER_ID = "fbiOfferID";
    public const string FASTBALL_OFFER_UID = "fbiOfferUID";
    public const string OVERRIDE_PRODUCT_NAME = "OV_ProductName";
    public const string RESOURCE_ID = "resource_id";
    public const string FORM_ID = "form_id";
    public const string FASTBALL_DISCOUNT = "fastballDiscount";
    public const string COMMISSION_JUNCTION_START_DATE = "commissionJunctionStartDate";
    public const string STACK_ID = "stackid";
    public const string TARGET_EXPIRATION_DATE = "target_expiration_date";
  }
}

