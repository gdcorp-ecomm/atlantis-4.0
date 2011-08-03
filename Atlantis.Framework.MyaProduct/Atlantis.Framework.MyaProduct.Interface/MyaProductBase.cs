using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Atlantis.Framework.MyaProduct.Interface
{
  public abstract class MyaProductBase
  {
    private const string PRODUCT_INFO_REQUEST_XML_FORMAT = "<GetProductInfoByUnifiedPFID><param name=\"n_gdshop_product_unifiedProductID\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetProductInfoByUnifiedPFID>";
    private const string RENEWAL_PRODUCTS_REQUEST_XML_FORMAT = "<BillingSyncProductList><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privatelabelResellerTypeID\" value=\"{1}\"/></BillingSyncProductList>";

    #region Abstract Properties

    protected abstract string AccountExpirationDateKey { get; }
    protected abstract string BillingResourceIdKey { get; }
    protected abstract string BundleProductIdKey { get; }
    protected abstract string BillingCreditsKey { get; }
    protected abstract string CommonNameKey { get; }
    protected abstract string OrionResourceIdKey { get; }
    protected abstract string IsPastDueKey { get; }
    protected abstract string LastExpirationDateKey { get; }
    protected abstract string NameSpaceKey { get; }
    protected abstract string ObsoleteResourceIdKey { get; }
    protected abstract string ParentBundleIdKey { get; }
    protected abstract string ProductIdKey { get; }
    protected abstract string ProductTypeIdKey { get; }
    protected abstract string RenewalProductIdKey { get; }
    protected abstract string IsFreeKey { get; }

    public abstract MyaProductType ProductType { get; }

    #endregion

    #region Properties

    private DateTime? _accountExpirationDate;
    public DateTime AccountExpirationDate
    {
      get
      {
        if (_accountExpirationDate == null)
        {
          if (IsPropertyInDictionary(AccountExpirationDateKey))
          {
            _accountExpirationDate = Convert.ToDateTime(PropertiesDictionary[AccountExpirationDateKey]);
          }
          else
          {
            throw new Exception(string.Format("Product AccountExpirationDate is null for this account. ResourceId: {0}, Key: {1}", BillingResourceId.ToString(CultureInfo.CurrentCulture), AccountExpirationDateKey));
          }
        }
        return _accountExpirationDate.Value;
      }
    }

    private int? _bundleProductId;
    public int? BundleProductId
    {
      get
      {
        if (_bundleProductId == null)
        {
          if (IsPropertyInDictionary(BundleProductIdKey))
          {
            _bundleProductId = Convert.ToInt32(PropertiesDictionary[BundleProductIdKey]);
          }
        }
        return _bundleProductId;
      }
    }

    private int? _billingCredits;
    public int BillingCredits
    {
      get
      {
        if (_billingCredits == null)
        {
          _billingCredits = 0;
          if (IsPropertyInDictionary(BillingCreditsKey))
          {
            _billingCredits = Convert.ToInt32(PropertiesDictionary[BillingCreditsKey]);
          }
        }
        return _billingCredits.Value;
      }
    }

    private string _commonName = string.Empty;
    public string CommonName
    {
      get
      {
        if (string.IsNullOrEmpty(_commonName))
        {
          if (IsPropertyInDictionary(CommonNameKey))
          {
            _commonName = Convert.ToString(PropertiesDictionary[CommonNameKey]);
          }
        }
        return _commonName;
      }
      set
      {
        _commonName = value;
      }
    }

    private string _orionResourceId = string.Empty;
    public string OrionResourceId
    {
      get
      {
        if (IsPropertyInDictionary(OrionResourceIdKey))
        {
          _orionResourceId = Convert.ToString(PropertiesDictionary[OrionResourceIdKey]);
        }
        return _orionResourceId;
      }
    }

    private bool? _isPastDue;
    public bool IsPastDue
    {
      get
      {
        if (_isPastDue == null)
        {
          _isPastDue = false;
          if (IsPropertyInDictionary(IsPastDueKey))
          {
            bool isPastDueFlag = Convert.ToBoolean(PropertiesDictionary[IsPastDueKey]);
            _isPastDue = isPastDueFlag || (AccountExpirationDate < DateTime.Today);
          }
        }
        return _isPastDue.Value;
      }
    }

    private DateTime? _lastExpirationDate;
    public DateTime? LastExpirationDate
    {
      get
      {
        if (_lastExpirationDate == null)
        {
          if (IsPropertyInDictionary(LastExpirationDateKey))
          {
            _lastExpirationDate = Convert.ToDateTime(PropertiesDictionary[LastExpirationDateKey]);
          }
        }
        return _lastExpirationDate;
      }
    }

    private string _nameSpace = string.Empty;
    public string Namespace
    {
      get
      {
        if (string.IsNullOrEmpty(_nameSpace))
        {
          if (IsPropertyInDictionary(NameSpaceKey))
          {
            _nameSpace = Convert.ToString(PropertiesDictionary[NameSpaceKey]);
          }
        }
        return _nameSpace;
      }
    }

    private int? _obsoleteResourceId;
    public int? ObsoleteResourceId
    {
      get
      {
        if (_obsoleteResourceId == null)
        {
          if (IsPropertyInDictionary(ObsoleteResourceIdKey))
          {
            _obsoleteResourceId = Convert.ToInt32(PropertiesDictionary[ObsoleteResourceIdKey]);
          }
        }
        return _obsoleteResourceId;
      }
    }

    private int? _parentBundleId;
    public int? ParentBundleId
    {
      get
      {
        if (_parentBundleId == null)
        {
          if (IsPropertyInDictionary(ParentBundleIdKey))
          {
            _parentBundleId = Convert.ToInt32(PropertiesDictionary[ParentBundleIdKey]);
          }
        }
        return _parentBundleId;
      }
    }

    private int? _productId;
    public int ProductId
    {
      get
      {
        if (_productId == null)
        {
          if (IsPropertyInDictionary(ProductIdKey))
          {
            _productId = Convert.ToInt32(PropertiesDictionary[ProductIdKey]);
          }
          else
          {
            throw new Exception(string.Format("Product ProductId is null for this account. ResourceID: {0}, Key: {1}", BillingResourceId.ToString(CultureInfo.CurrentCulture), ProductIdKey));
          }
        }
        return _productId.Value;
      }
    }

    private int? _productTypeId;
    public int ProductTypeId
    {
      get
      {
        if (_productTypeId == null)
        {
          _productTypeId = 0;
          if (IsPropertyInDictionary(ProductTypeIdKey))
          {
            _productTypeId = Convert.ToInt32(PropertiesDictionary[ProductTypeIdKey]);
          }
        }
        return _productTypeId.Value;
      }
    }

    private int? _renewalProductId;
    public int? RenewalProductId
    {
      get
      {
        if (_renewalProductId == null)
        {
          if (IsPropertyInDictionary(RenewalProductIdKey))
          {
            _renewalProductId = Convert.ToInt32(PropertiesDictionary[RenewalProductIdKey]);
          }
        }
        return _renewalProductId;
      }
    }

    private int? _billingResourceId;
    public int BillingResourceId
    {
      get
      {
        if (_billingResourceId == null)
        {
          if (IsPropertyInDictionary(BillingResourceIdKey))
          {
            _billingResourceId = Convert.ToInt32(PropertiesDictionary[BillingResourceIdKey]);
          }
          else
          {
            throw new Exception(string.Format("Product BillingResourceId is null for this account. Key: {0}", BillingResourceIdKey));
          }
        }
        return _billingResourceId.Value;
      }
    }

    public IDictionary<string, object> PropertiesDictionary { get; protected set; }

    private string _description;
    public string Description
    {
      get
      {
        if(string.IsNullOrEmpty(_description))
        {
          ParseProductInfoXml();
        }
        return _description;
      }
    }

    private RecurringPaymentType? _recurringPayment;
    public RecurringPaymentType RecurringPayment
    {
      get
      {
        if (_recurringPayment == null)
        {
          _recurringPayment = RecurringPaymentType.Unknown;
          ParseProductInfoXml();
        }
        return _recurringPayment.Value;
      }
    }

    private int? _numberOfPeriods;
    public int NumberOfPeriods
    {
      get
      {
        if (_numberOfPeriods == null)
        {
          _numberOfPeriods = 0;
          ParseProductInfoXml();
        }
        return _numberOfPeriods.Value;
      }
    }

    private bool? _isFree;
    public virtual bool IsFree
    {
      get
      {
        if(_isFree == null)
        {
          _isFree = false;
          if(IsPropertyInDictionary(IsFreeKey))
          {
            _isFree = Convert.ToBoolean(PropertiesDictionary[IsFreeKey]);
          }
        }
        return _isFree.Value;
      }
    }

    public virtual bool IsRenewable
    {
      get
      {
        return RenewalProductId != null && RenewalProductId > 0;
      }
    }

    private string _productInfoXml;
    protected string ProductInfoXml
    {
      get
      {
        if(string.IsNullOrEmpty(_productInfoXml))
        {
          try
          {
            _productInfoXml = DataCache.DataCache.GetCacheData(string.Format(PRODUCT_INFO_REQUEST_XML_FORMAT, ProductId, PrivateLabelId));
          }
          catch
          {
            _productInfoXml = string.Empty;
          }
        }
        return _productInfoXml;
      }
    }

    private string _productRenewalXml;
    protected string ProductRenewalXml
    {
      get
      {
        if (string.IsNullOrEmpty(_productRenewalXml))
        {
          try
          {
            _productRenewalXml = DataCache.DataCache.GetCacheData(string.Format(RENEWAL_PRODUCTS_REQUEST_XML_FORMAT, ProductId, PrivateLabelId));
          }
          catch
          {
            _productRenewalXml = string.Empty;
          }
        }
        return _productRenewalXml;
      }
    }

    protected int PrivateLabelId { get; private set; }

    #endregion

    protected MyaProductBase(int privateLabelId, IDictionary<string, object> propertiesDictionary)
    {
      PropertiesDictionary = propertiesDictionary;
      PrivateLabelId = privateLabelId;
    }

    protected bool IsPropertyInDictionary(string key)
    {
      return PropertiesDictionary.ContainsKey(key) && PropertiesDictionary[key] != null && !(PropertiesDictionary[key] is DBNull);
    }

    private void ParseProductInfoXml()
    {
      if(!string.IsNullOrEmpty(ProductInfoXml))
      {
        using (StringReader sr = new StringReader(ProductInfoXml))
        {
          using (XmlReader reader = XmlReader.Create(sr))
          {
            while (reader.Read())
            {
              if (reader.Name == "item")
              {
                while (reader.MoveToNextAttribute())
                {
                  switch (reader.Name)
                  {
                    case "numberOfPeriods":
                      int numPeriods;
                      if (int.TryParse(reader.Value, out numPeriods))
                      {
                        _numberOfPeriods = numPeriods;
                      }
                      break;
                    case "recurring_payment":
                      if (reader.Value == "monthly")
                        _recurringPayment = RecurringPaymentType.Monthly;
                      else if (reader.Value == "annual")
                        _recurringPayment = RecurringPaymentType.Annual;
                      else if (reader.Value == "semiannual")
                        _recurringPayment = RecurringPaymentType.SemiAnnual;
                      else if (reader.Value == "quarterly")
                        _recurringPayment = RecurringPaymentType.Quarterly;
                      else
                        _recurringPayment = RecurringPaymentType.Unknown;
                      break;
                    case "name":
                      _description = reader.Value;
                      break;

                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
