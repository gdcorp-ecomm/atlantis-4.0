using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Atlantis.Framework.Interface;
using System.Xml;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class MyaProduct : IComparable<MyaProduct>
  {
    #region NameSpace Enum
    public enum NameSpace
    {
      SSLCert,
      trafblazer,
    }
    #endregion //Namespace Enum

    #region Properties
    #region Base Class Properties

    private bool _autoRenewFlag;
    private DateTime _accountExpirationDate;
    private int _billingCredits;
    private int? _bundlePfId;
    private string _commonName = string.Empty;
    private string _description;
    private DateTime _expirationDate;
    private string _orionResourceId = string.Empty;
    private bool _isFree;
    private bool _isPastDue;
    private DateTime? _lastExpirationDate;
    private string _namespace;
    private int _numberOfPeriods;
    private int? _obsoleteResourceId;
    private int? _parentBundleId;
    private int _productId;  // convert to unified if necessary
    private int? _ppShopperProfileId;
    private int _productTypeId;
    private string _recurringPayment;
    private int? _renewalPfId;
    private int _billingResourceId;

    public bool AutoRenewFlag
    {
      [DebuggerStepThrough]
      get { return _autoRenewFlag; }
      set { _autoRenewFlag = value; }
    }

    public DateTime AccountExpirationDate
    {
      [DebuggerStepThrough]
      get { return _accountExpirationDate; }
      set { _accountExpirationDate = value; }
    }

    public int? BundlePfId
    {
      [DebuggerStepThrough]
      get { return _bundlePfId; }
      set { _bundlePfId = value; }
    }

    public int Credits
    {
      [DebuggerStepThrough]
      get { return _billingCredits; }
      set { _billingCredits = value; }
    }

    public string CommonName
    {
      [DebuggerStepThrough]
      get { return _commonName; }
      set { _commonName = value; }
    }

    public string Description
    {
      [DebuggerStepThrough]
      get { return _description; }
      set { _description = value; }
    }

    public DateTime ExpirationDate
    {
      [DebuggerStepThrough]
      get { return _expirationDate; }
      set { _expirationDate = value; }
    }

    public string OrionResourceId
    {
      [DebuggerStepThrough]
      get { return _orionResourceId; }
      set { _orionResourceId = value; }
    }

    public bool IsFree
    {
      [DebuggerStepThrough]
      get { return _isFree; }
      set { _isFree = value; }
    }

    public bool IsPastDue
    {
      [DebuggerStepThrough]
      get { return _isPastDue; }
      set { _isPastDue = value; }
    }

    public DateTime? LastExpirationDate
    {
      [DebuggerStepThrough]
      get { return _lastExpirationDate; }
      set { _lastExpirationDate = value; }
    }

    public string Namespace
    {
      [DebuggerStepThrough]
      get { return _namespace; }
      set { _namespace = value; }
    }

    public int NumberOfPeriods
    {
      [DebuggerStepThrough]
      get { return _numberOfPeriods; }
      set { _numberOfPeriods = value; }
    }

    public int? ObsoleteResourceId
    {
      [DebuggerStepThrough]
      get { return _obsoleteResourceId; }
      set { _obsoleteResourceId = value; }
    }

    public int? ParentBundleId
    {
      [DebuggerStepThrough]
      get { return _parentBundleId; }
      set { _parentBundleId = value; }
    }

    public int ProductId
    {
      [DebuggerStepThrough]
      get { return _productId; }
      set { _productId = value; }
    }

    public int? PpShopperProfileId
    {
      get { return _ppShopperProfileId; }
      set { _ppShopperProfileId = value; }
    }

    public int ProductTypeId
    {
      [DebuggerStepThrough]
      get { return _productTypeId; }
      set { _productTypeId = value; }
    }

    public string RecurringPayment
    {
      [DebuggerStepThrough]
      get { return _recurringPayment; }
      set { _recurringPayment = value; }
    }

    public int? RenewalPfId
    {
      [DebuggerStepThrough]
      get { return _renewalPfId; }
      set { _renewalPfId = value; }
    }

    public int BillingResourceId
    {
      [DebuggerStepThrough]
      get { return _billingResourceId; }
      set { _billingResourceId = value; }
    }
    #endregion //Base Class Properties

    #region CustomerManager Properties

    private bool? _canRenewFreeProducts;

    public bool? CanRenewFreeProducts
    {
      get { return _canRenewFreeProducts; }
      set { _canRenewFreeProducts = value; }
    }
    #endregion //CustomerManager Properties

    #region WebsiteTonight Properties

    private int? _customPageLayoutPfId;
    private int? _designPfId;
    private int? _designStatus;
    private bool? _hasMaintenance;
    private string _orderId = string.Empty;
    private int? _rowId;
    private int? _serviceFeePfId;

    public int? CustomPageLayoutPfId
    {
      get { return _customPageLayoutPfId; }
      set { _customPageLayoutPfId = value; }
    }

    public int? DesignPfId
    {
      get { return _designPfId; }
      set { _designPfId = value; }
    }

    public int? DesignStatus
    {
      get { return _designStatus; }
      set { _designStatus = value; }
    }

    public bool? HasMaintenance
    {
      get { return _hasMaintenance; }
      set { _hasMaintenance = value; }
    }

    public string OrderId
    {
      get { return _orderId; }
      set { _orderId = value; }
    }

    public int? RowId
    {
      get { return _rowId; }
      set { _rowId = value; }
    }

    public int? ServiceFeePfId
    {
      get { return _serviceFeePfId; }
      set { _serviceFeePfId = value; }
    }
    #endregion //WebsiteTonight Properties

    #region Express Email Marketing Properties

    private int? _activeQuota;
    private int? _addOnQuota;
    private int? _customerId;
    private int? _entityId;
    private int? _parentBundleProductTypeId;
    private int? _pendingQuota;
    private DateTime? _startDate;

    public int? ActiveQuota 
    {
      [DebuggerStepThrough]
      get { return _activeQuota; } 
      set { _activeQuota = value; } 
    }
    
    public int? AddOnQuota 
    {
      [DebuggerStepThrough]
      get { return _addOnQuota; } 
      set { _addOnQuota = value; }
    }

    public int? CustomerId
    {
      [DebuggerStepThrough]
      get { return _customerId; }
      set { _customerId = value; }
    }

    public int? EntityId
    {
      [DebuggerStepThrough]
      get { return _entityId; }
      set { _entityId = value; }
    }

    public int? ParentBundleProductTypeId 
    { 
      [DebuggerStepThrough]
      get { return _parentBundleProductTypeId; } 
      set { _parentBundleProductTypeId = value; } 
    }

    public int? PendingQuota 
    { 
      [DebuggerStepThrough]
      get { return _pendingQuota; } 
      set { _pendingQuota = value; } 
    }

    public DateTime? StartDate 
    { 
      [DebuggerStepThrough]
      get { return _startDate; } 
      set { _startDate = value; }
    }
    #endregion //Express Email Marketing Properties

    #region Reseller Properties
    private int? _privateLabelGroupType;

    public int? PrivateLabelGroupType
    {
      get { return _privateLabelGroupType; }
      set { _privateLabelGroupType = value; }
    }
    #endregion //Reseller Properties

    #region Traffic Blazer Properties

    private DateTime? _dateSubmitted;
    private int? _userWebsiteId;

    public DateTime? DateSubmitted
    {
      get { return _dateSubmitted; }
      set { _dateSubmitted = value; }
    }

    public int? UserWebsiteId
    {
      get { return _userWebsiteId; }
      set { _userWebsiteId = value; }
    }
    #endregion //Traffic Blazer Properties

    #region Merchant Account Properties

    private int? _merchantAccountId;
    private DateTime? _createDate;
    private string _authenticationGUID;
    private string _applicationDescription;
    private string _supportPhone;
    private string _referralId;

    public int? MerchantAccountId
    {
      get { return _merchantAccountId; }
      set { _merchantAccountId = value; }
    }

    public DateTime? CreateDate
    {
      get { return _createDate; }
      set { _createDate = value; }
    }

    public string AuthenticationGUID
    {
      get { return _authenticationGUID; }
      set { _authenticationGUID = value; }
    }

    public string ApplicationDescription
    {
      get { return _applicationDescription; }
      set { _applicationDescription = value; }
    }

    public string SupportPhone
    {
      get { return _supportPhone; }
      set { _supportPhone = value; }
    }

    public string ReferralId
    {
      get { return _referralId; }
      set { _referralId = value; }
    }
    #endregion //Merchant Account Properties

    #endregion //Properties

    #region Constructors

    public MyaProduct() { }
    
    #endregion

    #region Methods
    
    public virtual MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct mya = new MyaProduct();  
      int tmp = 0;

      mya.BillingResourceId = Convert.ToInt32(dr["resource_id"], CultureInfo.CurrentCulture);
      mya.ProductId = Convert.ToInt32(dr["pf_id"], CultureInfo.CurrentCulture);

      // Account Expiration date should never be null;
      if (dr["account_expiration_date"] == DBNull.Value)
      {
        throw new AtlantisException(myaProductRequestData,
          "MyaProductLite::PopulateObjectFromDB",
          "Product account_expiration_date is null for this account. " + "ResourceID: " + mya.BillingResourceId.ToString(CultureInfo.CurrentCulture),
          string.Empty);
      }
      else
      {
        mya.AccountExpirationDate = Convert.ToDateTime(dr["account_expiration_date"], CultureInfo.CurrentCulture);
      }

      mya.Credits = Convert.ToInt32(dr["billing_credits"], CultureInfo.CurrentCulture);
      mya.BundlePfId = dr["bundle_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["bundle_pf_id"], CultureInfo.CurrentCulture);
      mya.ProductTypeId = dr["gdshop_product_typeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["gdshop_product_typeID"], CultureInfo.CurrentCulture);
      mya.CommonName = dr["commonName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["commonName"], CultureInfo.CurrentCulture).Trim();

      mya.Description = string.Empty;

      // Expiration date should never be null;
      if (dr["expiration_date"] == DBNull.Value)
      {
        throw new AtlantisException(myaProductRequestData,
          "MyaProductLite::PopulateObjectFromDB",
          "Product expiration_date is null for this account. " + "ResourceID: " + mya.BillingResourceId.ToString(CultureInfo.CurrentCulture),
          string.Empty);
      }
      else
      {
        mya.ExpirationDate = Convert.ToDateTime(dr["expiration_date"], CultureInfo.CurrentCulture);
      }

      mya.OrionResourceId = Convert.ToString(dr["externalResourceID"], CultureInfo.CurrentCulture);
      mya.IsFree = false;
      if (hasIsFreeColumn)
      {
        mya.IsFree = Convert.ToInt32(dr["isFree"], CultureInfo.CurrentCulture) == 0 ? false : true;
      }
      
      mya.IsPastDue = Convert.ToInt32(dr["isPastDue"], CultureInfo.CurrentCulture) == 0 ? false : true;
      mya.LastExpirationDate = dr["last_expiration_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["last_expiration_date"], CultureInfo.CurrentCulture);
      mya.Namespace = dr["namespace"] == DBNull.Value ? null : dr["namespace"].ToString().Trim();
      mya.ObsoleteResourceId = int.TryParse(dr["obsoleteResourceID"].ToString(), out tmp) ? (int?)tmp : null;
      mya.ParentBundleId = dr["parent_bundle_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["parent_bundle_id"], CultureInfo.CurrentCulture);
      mya.RenewalPfId = dr["renewal_pf_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["renewal_pf_id"], CultureInfo.CurrentCulture);
      mya.AutoRenewFlag = Convert.ToInt32(dr["autoRenewFlag"], CultureInfo.CurrentCulture) == 0 ? false : true;
      mya.NumberOfPeriods = 0;
      mya.PpShopperProfileId = dr["pp_shopperProfileID"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["pp_shopperProfileID"], CultureInfo.CurrentCulture);
      mya.RecurringPayment = string.Empty;
      return mya;
    }
    #endregion

    #region IComparable<MyaProduct> Members

    public int CompareTo(MyaProduct x)
    {
        return string.Compare(CommonName, x.CommonName);      
    }

    #endregion
  }
}