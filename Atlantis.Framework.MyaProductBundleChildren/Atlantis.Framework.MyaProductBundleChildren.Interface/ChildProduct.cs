using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MyaProductBundleChildren.Interface
{
  public class ChildProduct
  {
    #region Database Column Keys
    private static string BillingResourceIdKey { get { return "child_resource_id"; } }
    private static string CommonNameKey { get { return "commonName"; } }
    private static string CustomerIdKey { get { return "CustomerID"; } }
    private static string OrionResourceIdKey { get { return "externalResourceID"; } }
    private static string ProductIdKey { get { return "pf_id"; } }
    private static string ProductTypeIdKey { get { return "child_product_type_id"; } }
    private static string RecurringPaymentKey { get { return "recurring_payment"; } }
    private static string StartDateKey { get { return "start_date"; } }
    #endregion

    #region Properties
    public IDictionary<string, object> PropertiesDictionary { get; protected set; }

    private int? _billingResourceId;
    public int BillingResourceId
    {
      get
      {
        if (!_billingResourceId.HasValue)
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

    private int? _customerId;
    public int CustomerId
    {
      get
      {
        if (!_customerId.HasValue)
        {
          _customerId = -1;
          if (IsPropertyInDictionary(CustomerIdKey))
          {
            _customerId = Convert.ToInt32(PropertiesDictionary[CustomerIdKey]);
          }
        }
        return _customerId.Value;
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

    public int ParentBundleId { get; private set; }

    public int ParentBundleProductTypeId
    {
      get { return 65; }
    }

    private int? _productId;
    public int ProductId
    {
      get
      {
        if (_productId == null)
        {
          _productId = 0;
          if (IsPropertyInDictionary(ProductIdKey))
          {
            _productId = Convert.ToInt32(PropertiesDictionary[ProductIdKey]);
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
        if (!_productTypeId.HasValue)
        {
          if (IsPropertyInDictionary(ProductTypeIdKey))
          {
            _productTypeId = Convert.ToInt32(PropertiesDictionary[ProductTypeIdKey]);
          }
          else
          {
            throw new Exception(string.Format("Product ProductTypeId is null for this account. Key: {0}", ProductTypeIdKey));
          }
        }
        return _productTypeId.Value;
      }
    }

    private string _recurringPayment = string.Empty;
    public string RecurringPayment
    {
      get
      {
        if (string.IsNullOrEmpty(_recurringPayment))
        {
          if (IsPropertyInDictionary(RecurringPaymentKey))
          {
            _recurringPayment = Convert.ToString(PropertiesDictionary[RecurringPaymentKey]);
          }
        }
        return _recurringPayment;
      }
    }

    private DateTime? _startDate;
    public DateTime StartDate
    {
      get
      {
        if (!_startDate.HasValue)
        {
          _startDate = DateTime.MinValue;
          if (IsPropertyInDictionary(StartDateKey))
          {
            _startDate = Convert.ToDateTime(PropertiesDictionary[StartDateKey]);
          }
        }
        return _startDate.Value;
      }
    }

    public int? UserWebsiteId { get; set; }

    #endregion

    public ChildProduct(IDictionary<string, object> propertiesDictionary, int parentBillingResourceId)
    {
      ParentBundleId = parentBillingResourceId;
      PropertiesDictionary = propertiesDictionary;
    }

    private bool IsPropertyInDictionary(string key)
    {
      return PropertiesDictionary.ContainsKey(key) && PropertiesDictionary[key] != null && !(PropertiesDictionary[key] is DBNull);
    }
  }
}
