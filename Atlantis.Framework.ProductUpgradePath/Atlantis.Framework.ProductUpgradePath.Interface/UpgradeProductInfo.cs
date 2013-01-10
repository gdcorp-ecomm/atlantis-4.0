using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public class UpgradeProductInfo : Dictionary<string, int>
  {

    #region DataFields
    private const string UNIFIED_PRODUCT_ID = "catalog_productUnifiedID";
    private const string PERIOD_COUNT = "numberOfPeriods";
    private const string DEFAULT_DURATION_TYPE_ID = "gdshop_productDefaultDurationTypeID";
    private const string DEFAULT_DURATION = "defaultDurationValue";
    private const string DEFAULT_QUANTITY_TYPE_ID = "gdshop_productDefaultQuantityTypeID";
    private const string DEFAULT_QUANTITY = "defaultQuantityValue";
    private const string RECURRING_PAYMENT = "recurring_payment";
    private const string FULFILLMENT_METHOD = "gdshop_product_type_fulfillmentMethodID";
    #endregion

    public const string MONTHLY = "monthly";
    public const string QUARTERLY = "quarterly";
    public const string SEMIANUUAL = "semiannual";
    public const string ANNUAL = "annual";

    private string _recurringMethod = string.Empty;

    public UpgradeProductInfo(IDataReader currentProduct)
    {
      this[UNIFIED_PRODUCT_ID] = FieldReader.ReadField<int>(currentProduct, UNIFIED_PRODUCT_ID, -1);
      this[PERIOD_COUNT] = FieldReader.ReadField<int>(currentProduct, PERIOD_COUNT, -1);
      this[DEFAULT_DURATION_TYPE_ID] = FieldReader.ReadField<int>(currentProduct, DEFAULT_DURATION_TYPE_ID, -1);
      this[DEFAULT_DURATION] = FieldReader.ReadField<int>(currentProduct, DEFAULT_DURATION, -1);
      this[DEFAULT_QUANTITY_TYPE_ID] = FieldReader.ReadField<int>(currentProduct, DEFAULT_QUANTITY_TYPE_ID, -1);
      this[DEFAULT_QUANTITY] = FieldReader.ReadField<int>(currentProduct, DEFAULT_QUANTITY, -1);
      _recurringMethod = FieldReader.ReadField<string>(currentProduct, RECURRING_PAYMENT, string.Empty);
      this[FULFILLMENT_METHOD] = FieldReader.ReadField<int>(currentProduct, FULFILLMENT_METHOD, -1);
    }

    public bool IsRecurringType
    {
      get
      {
        bool isRecurring = false;
        return isRecurring;
      }
    }

    public int NumberOfMonthsForRecurringType
    {
      get
      {
        string recurringType = _recurringMethod;
        int monthAmount = 0;
        if (!string.IsNullOrWhiteSpace(recurringType))
        {
          int monthsPerRecurringType;
          switch (recurringType.ToLowerInvariant())
          {
            case ANNUAL:
              monthAmount = 12;
              break;
            case MONTHLY:
              monthAmount = 1;
              break;
            case QUARTERLY:
              monthAmount = 3;
              break;
            case SEMIANUUAL:
              monthAmount = 6;
              break;
            default:
              monthAmount = 12;
              break;
          }
        }
        return monthAmount;
      }
    }

    #region Fields

    public string RecurringMethod
    {
      get
      {
        return _recurringMethod;
      }
    }

    public int ProductID
    {
      get
      {
        return this[UNIFIED_PRODUCT_ID];
      }
    }

    public int DefaultDurationTypeID
    {
      get
      {
        return this[DEFAULT_DURATION_TYPE_ID];
      }
    }

    public int DefaultDuration
    {
      get
      {
        return this[DEFAULT_DURATION];
      }
    }

    public int DefaultQuantityTypeID
    {
      get
      {
        return this[DEFAULT_QUANTITY_TYPE_ID];
      }
    }

    public int DefaultQuantity
    {
      get
      {
        return this[DEFAULT_QUANTITY];
      }
    }

    public int PeriodCount
    {
      get
      {
        return this[PERIOD_COUNT];
      }
    }

    private string _displayTerm = string.Empty;
    public string DisplayTerm
    {
      get
      {
        return _displayTerm;
      }
      set
      {
        _displayTerm = value;
      }
    }
    #endregion
  }
}
