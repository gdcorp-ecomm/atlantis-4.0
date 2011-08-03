using System;
using System.Collections.Generic;

namespace Atlantis.Framework.ResourceBillingInfo.Interface
{
  public class GdshopResourceBillingInfo
  {
    #region Column Keys
    private const string BillingResourceIdColumnKey = "resource_id";
    private const string ProductTypeColumnKey = "gdshop_product_typeID";
    private const string ShopperIdColumnKey = "shopper_id";
    private const string OrderIdColumnKey = "order_id";
    private const string RowIdColumnKey = "row_id";
    private const string PrivateLabelIdColumnKey = "PrivateLabelID";
    private const string PfIdColumnKey = "pf_id";
    private const string BillingStatusColumnKey = "gdshop_billing_statusID";
    private const string BillingAttemptIdColumnKey = "gdshop_billing_attemptID";
    private const string BillingDateColumnKey = "billing_date";
    private const string LastRenewalDateColumnKey = "last_renewal_date";
    private const string ExpirationDateColumnKey = "expiration_date";
    private const string PpShopperProfileIdColumnKey = "pp_shopperProfileID";
    private const string AutoRenewFlagColumnKey = "autoRenewFlag";
    private const string IsRenewalPriceLockedColumnKey = "isRenewalPriceLocked";
    private const string OriginalListPriceColumnKey = "originalListPrice";
    private const string ParentBundleResourceIdColumnKey = "parent_bundle_id";
    private const string ParentProductTypeIdColumnKey = "parent_product_typeID";
    private const string FreeProductPackageIdColumnKey = "gdshop_free_product_packageID";
    private const string CancelDateColumnKey = "cancel_date";
    private const string CancelByColumnKey = "cancel_by";
    private const string CreateDateColumnKey = "createDate";
    private const string LastExpirationDateColumnKey = "last_expiration_date";
    private const string BillingCreditsColumnKey = "billing_credits";
    private const string QuantityColumnKey = "quantity";
    private const string ParentResourceIdColumnKey = "parent_resource_id";
    #endregion

    #region Xml Attribute Keys
    private const string BillingResourceIdXmlKey = "billingResourceId";
    private const string ProductTypeXmlKey = "productTypeId";
    private const string ShopperIdXmlKey = "shopperId";
    private const string OrderIdXmlKey = "orderId";
    private const string RowIdXmlKey = "rowId";
    private const string PrivateLabelIdXmlKey = "privateLabelId";
    private const string PfIdXmlKey = "pfid";
    private const string BillingStatusXmlKey = "billingStatusId";
    private const string BillingAttemptIdXmlKey = "billingAttemptId";
    private const string BillingDateXmlKey = "billingDate";
    private const string LastRenewalDateXmlKey = "lastRenewalDate";
    private const string ExpirationDateXmlKey = "expirationDate";
    private const string PpShopperProfileIdXmlKey = "ppShopperProfileId";
    private const string AutoRenewFlagXmlKey = "isAutoRenew";
    private const string IsRenewalPriceLockedXmlKey = "isRenewalPriceLocked";
    private const string OriginalListPriceXmlKey = "originalListPrice";
    private const string ParentBundleResourceIdXmlKey = "parentBundleResourceId";
    private const string ParentProductTypeIdXmlKey = "parentProductTypeId";
    private const string FreeProductPackageIdXmlKey = "freeProductPackageId";
    private const string CancelDateXmlKey = "cancelDate";
    private const string CancelByXmlKey = "cancelledBy";
    private const string CreateDateXmlKey = "createDate";
    private const string LastExpirationDateXmlKey = "lastExpirationDate";
    private const string BillingCreditsXmlKey = "billingCredits";
    private const string QuantityXmlKey = "quantity";
    private const string ParentResourceIdXmlKey = "parentResourceId";
    #endregion

    #region Properties

    // Database Properties
    public int BillingResourceId { get; private set; }
    public int ProductTypeId { get; private set; }
    public string OrderId { get; private set; }
    public int RowId { get; private set; }
    public int PrivateLabelId { get; private set; }
    public int PfId { get; private set; }
    public int BillingStatusId { get; private set; }
    public int BillingAttemptId { get; private set; }
    public DateTime BillingDate { get; private set; }
    public DateTime LastRenewalDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public int? PpShopperProfileId { get; private set; }
    public int OriginalListPrice { get; private set; }
    public int? ParentBundleResourceId { get; private set; }
    public int? ParentProductTypeId { get; private set; }
    public int? FreeProductPackageId { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public string CancelledBy { get; private set; }
    public DateTime? CreateDate { get; private set; }
    public DateTime? LastExpirationDate { get; private set; }
    public int BillingCredits { get; private set; }
    public int? Quantity { get; private set; }
    public string ShopperId { get; private set; }
    public int? ParentResourceId { get; private set; }

    // Derived Properties from Database Results
    public bool IsPastDue { get; private set; }             // True if BillingAttemptId != 0
    public bool IsFree { get; private set; }                // True if BillingStatusId == 2
    public bool IsAutoRenew { get; private set; }           // True if AutoRenewFlag == 1
    public bool IsRenewalPriceLocked { get; private set; }  // True if IsRenewalPriceLocked == 1

    private IDictionary<string, object> BillingPropertiesDictionary { get; set; }

    #endregion

    #region Constructors
    public GdshopResourceBillingInfo(IDictionary<string, object> billingPropertiesDictionary)
    {
      BillingPropertiesDictionary = billingPropertiesDictionary;
      BillingAttemptId = IsPropertyInDictionary(BillingAttemptIdColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingAttemptIdColumnKey]) : 0;
      BillingCredits = IsPropertyInDictionary(BillingCreditsColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingCreditsColumnKey]) : 0;
      BillingDate = IsPropertyInDictionary(BillingDateColumnKey) ? Convert.ToDateTime(BillingPropertiesDictionary[BillingDateColumnKey]) : DateTime.MinValue;
      BillingResourceId = IsPropertyInDictionary(BillingResourceIdColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingResourceIdColumnKey]) : 0;
      BillingStatusId = IsPropertyInDictionary(BillingStatusColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingStatusColumnKey]) : 0;
      CancelDate = IsPropertyInDictionary(CancelDateColumnKey) ?  (DateTime?)Convert.ToDateTime(BillingPropertiesDictionary[CancelDateColumnKey]) : null;
      CancelledBy = IsPropertyInDictionary(CancelByColumnKey) ? Convert.ToString(BillingPropertiesDictionary[CancelByColumnKey]) : null;
      CreateDate = IsPropertyInDictionary(CreateDateColumnKey) ? (DateTime?)Convert.ToDateTime(BillingPropertiesDictionary[CreateDateColumnKey]) : null;
      ExpirationDate = IsPropertyInDictionary(ExpirationDateColumnKey) ? Convert.ToDateTime(BillingPropertiesDictionary[ExpirationDateColumnKey]) : DateTime.MinValue;
      FreeProductPackageId = IsPropertyInDictionary(FreeProductPackageIdColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[FreeProductPackageIdColumnKey]) : null;
      IsAutoRenew = IsPropertyInDictionary(AutoRenewFlagColumnKey) ? Convert.ToBoolean(BillingPropertiesDictionary[AutoRenewFlagColumnKey]) : false;
      IsFree = BillingStatusId == 2;
      IsPastDue = BillingAttemptId != 0;
      IsRenewalPriceLocked = IsPropertyInDictionary(IsRenewalPriceLockedColumnKey) ? Convert.ToBoolean(BillingPropertiesDictionary[IsRenewalPriceLockedColumnKey]) : false;
      LastExpirationDate = IsPropertyInDictionary(LastExpirationDateColumnKey) ? (DateTime?)Convert.ToDateTime(BillingPropertiesDictionary[LastExpirationDateColumnKey]) : null;
      LastRenewalDate = IsPropertyInDictionary(LastRenewalDateColumnKey) ? Convert.ToDateTime(BillingPropertiesDictionary[LastRenewalDateColumnKey]) : DateTime.MinValue;
      OrderId = IsPropertyInDictionary(OrderIdColumnKey) ? Convert.ToString(BillingPropertiesDictionary[OrderIdColumnKey]) : string.Empty;
      OriginalListPrice = IsPropertyInDictionary(OriginalListPriceColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[OriginalListPriceColumnKey]) : 0;
      ParentBundleResourceId = IsPropertyInDictionary(ParentBundleResourceIdColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[ParentBundleResourceIdColumnKey]) : null;
      ParentProductTypeId = IsPropertyInDictionary(ParentProductTypeIdColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[ParentProductTypeIdColumnKey]) : null;
      ParentResourceId = IsPropertyInDictionary(ParentResourceIdColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[ParentResourceIdColumnKey]) : null;
      PfId = IsPropertyInDictionary(PfIdColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[PfIdColumnKey]) : 0;
      PpShopperProfileId = IsPropertyInDictionary(PpShopperProfileIdColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[PpShopperProfileIdColumnKey]) : null;
      PrivateLabelId = IsPropertyInDictionary(PrivateLabelIdColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[PrivateLabelIdColumnKey]) : 0;
      ProductTypeId = IsPropertyInDictionary(ProductTypeColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[ProductTypeColumnKey]) : 0;
      Quantity = IsPropertyInDictionary(QuantityColumnKey) ? (int?)Convert.ToInt32(BillingPropertiesDictionary[QuantityColumnKey]) : null;
      RowId = IsPropertyInDictionary(RowIdColumnKey) ? Convert.ToInt32(BillingPropertiesDictionary[RowIdColumnKey]) : 0;
      ShopperId = IsPropertyInDictionary(ShopperIdColumnKey) ? Convert.ToString(BillingPropertiesDictionary[ShopperIdColumnKey]) : string.Empty;
    }

    public GdshopResourceBillingInfo(IDictionary<string, object> billingPropertiesDictionary, bool fromXML)
    {
      BillingPropertiesDictionary = billingPropertiesDictionary;
      BillingAttemptId = IsPropertyInDictionary(BillingAttemptIdXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingAttemptIdXmlKey]) : 0;
      BillingCredits = IsPropertyInDictionary(BillingCreditsXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingCreditsXmlKey]) : 0;
      BillingDate = IsPropertyInDictionary(BillingDateXmlKey) ? Convert.ToDateTime(BillingPropertiesDictionary[BillingDateXmlKey]) : DateTime.MinValue;
      BillingResourceId = IsPropertyInDictionary(BillingResourceIdXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingResourceIdXmlKey]) : 0;
      BillingStatusId = IsPropertyInDictionary(BillingStatusXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[BillingStatusXmlKey]) : 0;
      CancelDate = IsPropertyInDictionary(CancelDateXmlKey) ? GetDateTimeFromXmlString(BillingPropertiesDictionary[CancelDateXmlKey].ToString()) : null;
      CancelledBy = IsPropertyInDictionary(CancelByXmlKey) ? Convert.ToString(BillingPropertiesDictionary[CancelByXmlKey]) : null;
      CreateDate = IsPropertyInDictionary(CreateDateXmlKey) ? GetDateTimeFromXmlString(BillingPropertiesDictionary[CreateDateXmlKey].ToString()) : null;
      ExpirationDate = IsPropertyInDictionary(ExpirationDateXmlKey) ? Convert.ToDateTime(BillingPropertiesDictionary[ExpirationDateXmlKey]) : DateTime.MinValue;
      FreeProductPackageId = IsPropertyInDictionary(FreeProductPackageIdXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[FreeProductPackageIdXmlKey].ToString()) : null;
      IsAutoRenew = IsPropertyInDictionary(AutoRenewFlagXmlKey) ? Convert.ToBoolean(BillingPropertiesDictionary[AutoRenewFlagXmlKey]) : false;
      IsFree = BillingStatusId == 2;
      IsPastDue = BillingAttemptId != 0;
      IsRenewalPriceLocked = IsPropertyInDictionary(IsRenewalPriceLockedXmlKey) ? Convert.ToBoolean(BillingPropertiesDictionary[IsRenewalPriceLockedXmlKey]) : false;
      LastExpirationDate = IsPropertyInDictionary(LastExpirationDateXmlKey) ? GetDateTimeFromXmlString(BillingPropertiesDictionary[LastExpirationDateXmlKey].ToString()) : null;
      LastRenewalDate = IsPropertyInDictionary(LastRenewalDateXmlKey) ? Convert.ToDateTime(BillingPropertiesDictionary[LastRenewalDateXmlKey]) : DateTime.MinValue;
      OrderId = IsPropertyInDictionary(OrderIdXmlKey) ? Convert.ToString(BillingPropertiesDictionary[OrderIdXmlKey]) : string.Empty;
      OriginalListPrice = IsPropertyInDictionary(OriginalListPriceXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[OriginalListPriceXmlKey]) : 0;
      ParentBundleResourceId = IsPropertyInDictionary(ParentBundleResourceIdXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[ParentBundleResourceIdXmlKey].ToString()) : null;
      ParentProductTypeId = IsPropertyInDictionary(ParentProductTypeIdXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[ParentProductTypeIdXmlKey].ToString()) : null;
      ParentResourceId = IsPropertyInDictionary(ParentResourceIdXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[ParentResourceIdXmlKey].ToString()) : null;
      PfId = IsPropertyInDictionary(PfIdXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[PfIdXmlKey]) : 0;
      PpShopperProfileId = IsPropertyInDictionary(PpShopperProfileIdXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[PpShopperProfileIdXmlKey].ToString()) : null;
      PrivateLabelId = IsPropertyInDictionary(PrivateLabelIdXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[PrivateLabelIdXmlKey]) : 0;
      ProductTypeId = IsPropertyInDictionary(ProductTypeXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[ProductTypeXmlKey]) : 0;
      Quantity = IsPropertyInDictionary(QuantityXmlKey) ? GetIntegerFromXmlString(BillingPropertiesDictionary[QuantityXmlKey].ToString()) : null;
      RowId = IsPropertyInDictionary(RowIdXmlKey) ? Convert.ToInt32(BillingPropertiesDictionary[RowIdXmlKey]) : 0;
      ShopperId = IsPropertyInDictionary(ShopperIdXmlKey) ? Convert.ToString(BillingPropertiesDictionary[ShopperIdXmlKey]) : string.Empty;
    }
    #endregion

    #region Helper Methods

    private bool IsPropertyInDictionary(string key)
    {
      return BillingPropertiesDictionary.ContainsKey(key) && BillingPropertiesDictionary[key] != null && !(BillingPropertiesDictionary[key] is DBNull);
    }

    private static int? GetIntegerFromXmlString(string value)
    {
      int? result = null;
      int intValue;

      if (int.TryParse(value, out intValue))
      {
        result = intValue;
      }

      return result;
    }

    private static DateTime? GetDateTimeFromXmlString(string value)
    {
      DateTime? result = null;
      DateTime datetimeValue;

      if (DateTime.TryParse(value, out datetimeValue))
      {
        result = datetimeValue;
      }

      return result;
    }
    #endregion
  }
}
