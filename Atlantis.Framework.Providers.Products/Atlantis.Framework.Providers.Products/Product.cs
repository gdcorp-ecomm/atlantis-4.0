using System;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Products
{
  public class Product : IProduct
  {
    private IProductInfo _productInfo;
    private int _productId;
    private int _privateLabelId;
    private ICurrencyProvider _currencyProvider;
    private Lazy<Dictionary<RecurringPaymentUnitType, double>> _durationUnits;
   
    internal Product(int productId, int privateLabelId, ICurrencyProvider currencyProvider)
    {
      _productId = productId;
      _privateLabelId = privateLabelId;
      _currencyProvider = currencyProvider;
      _productInfo = new ProductInfo(productId, _privateLabelId);
      _durationUnits = new Lazy<Dictionary<RecurringPaymentUnitType, double>>(() => { return CalculateDurationUnits(); });
    }

    public IProductInfo Info
    {
      get { return _productInfo; }
    }

    public int ProductId
    {
      get { return _productId; }
    }

    #region Duration Units

    private Dictionary<RecurringPaymentUnitType, double> CalculateDurationUnits()
    {
      Dictionary<RecurringPaymentUnitType, double> result = new Dictionary<RecurringPaymentUnitType, double>();

      switch (DurationUnit)
      {
        case RecurringPaymentUnitType.Monthly:
          result[RecurringPaymentUnitType.Monthly] = Duration;
          result[RecurringPaymentUnitType.Quarterly] = Duration / 3.0;
          result[RecurringPaymentUnitType.SemiAnnual] = Duration / 6.0;
          result[RecurringPaymentUnitType.Annual] = Duration / 12.0;
          break;
        case RecurringPaymentUnitType.Annual:
          result[RecurringPaymentUnitType.Monthly] = Duration * 12.0;
          result[RecurringPaymentUnitType.Quarterly] = Duration * 4.0;
          result[RecurringPaymentUnitType.SemiAnnual] = Duration * 2.0;
          result[RecurringPaymentUnitType.Annual] = Duration;
          break;
        case RecurringPaymentUnitType.SemiAnnual:
          result[RecurringPaymentUnitType.Monthly] = Duration * 6.0;
          result[RecurringPaymentUnitType.Quarterly] = Duration * 2.0;
          result[RecurringPaymentUnitType.SemiAnnual] = Duration;
          result[RecurringPaymentUnitType.Annual] = Duration / 2.0;
          break;
        case RecurringPaymentUnitType.Quarterly:
          result[RecurringPaymentUnitType.Monthly] = Duration * 3.0;
          result[RecurringPaymentUnitType.Quarterly] = Duration;
          result[RecurringPaymentUnitType.SemiAnnual] = Duration / 2.0;
          result[RecurringPaymentUnitType.Annual] = Duration / 4.0;
          break;
        default:
          string message = "Product DurationUnit Error";
          string data = string.Concat("ProductId=", ProductId.ToString(), ":DurationUnit=", DurationUnit.ToString());
          AtlantisException ex = new AtlantisException("Product.CalculateDurationUnits", "30", message, data, null, null);
          Engine.Engine.LogAtlantisException(ex);
          result[RecurringPaymentUnitType.Monthly] = Duration;
          result[RecurringPaymentUnitType.Quarterly] = Duration;
          result[RecurringPaymentUnitType.SemiAnnual] = Duration;
          result[RecurringPaymentUnitType.Annual] = Duration;
          break;
      }

      return result;
    }

    public double Years
    {
      get { return _durationUnits.Value[RecurringPaymentUnitType.Annual]; }
    }

    public int Months
    {
      get { return (int)_durationUnits.Value[RecurringPaymentUnitType.Monthly]; }
    }

    public double Quarters
    {
      get { return _durationUnits.Value[RecurringPaymentUnitType.Quarterly]; }
    }

    public double HalfYears
    {
      get { return _durationUnits.Value[RecurringPaymentUnitType.SemiAnnual]; }
    }

    public int Duration
    {
      get
      {
        return _productInfo.NumberOfPeriods;
      }
    }

    public RecurringPaymentUnitType DurationUnit
    {
      get
      {
        return _productInfo.RecurringPayment;
      }
    }

    #endregion

    #region OnSale

    public bool IsOnSale
    {
      get { return _currencyProvider.IsProductOnSale(ProductId); }
    }

    public bool GetIsOnSale(int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
    {
      return _currencyProvider.IsProductOnSale(ProductId, shopperPriceType, transactionCurrency, isc);
    }

    #endregion

    #region Pricing

    private static int ConvertToInt32NoOverflow(double value)
    {
      if (value >= int.MaxValue)
      {
        return int.MaxValue;
      }
      else if (value <= int.MinValue)
      {
        return int.MinValue;
      }

      return (int)value;
    }

    private ICurrencyPrice GetPeriodPrice(ICurrencyPrice price, RecurringPaymentUnitType durationUnit, PriceRoundingType roundingType)
    {
      ICurrencyPrice result = price;

      if (durationUnit != RecurringPaymentUnitType.Unknown)
      {
        double periods = _durationUnits.Value[durationUnit];

        if ((periods > 0) && (periods != 1.0))
        {
          int periodPrice;
          if (roundingType == PriceRoundingType.RoundFractionsUpProperly)
          {
            periodPrice = ConvertToInt32NoOverflow(Math.Ceiling(price.Price / periods));
          }
          else
          {
            periodPrice = ConvertToInt32NoOverflow(Math.Floor(price.Price / periods));
          }
          result = _currencyProvider.NewCurrencyPrice(periodPrice, price.CurrencyInfo, price.Type);
        }
      }

      return result;
    }

    private ICurrencyPrice _listPrice;
    public ICurrencyPrice ListPrice
    {
      get
      {
        if (_listPrice == null)
        {
          _listPrice = _currencyProvider.GetListPrice(ProductId);
        }
        return _listPrice;
      }
    }

    public ICurrencyPrice GetListPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      ICurrencyPrice price = _currencyProvider.GetListPrice(ProductId, shopperPriceType, transactionCurrency);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    private ICurrencyPrice _currentPrice;
    public ICurrencyPrice CurrentPrice
    {
      get
      {
        if (_currentPrice == null)
        {
          _currentPrice = _currencyProvider.GetCurrentPrice(ProductId);
        }
        return _currentPrice;
      }
    }

    public ICurrencyPrice GetCurrentPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      ICurrencyPrice price = _currencyProvider.GetCurrentPrice(ProductId, shopperPriceType, transactionCurrency, isc);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int quantity)
    {
      return _currencyProvider.GetCurrentPriceByQuantity(ProductId, quantity);
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int quantity, RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      ICurrencyPrice price = _currencyProvider.GetCurrentPriceByQuantity(ProductId, quantity, shopperPriceType, transactionCurrency);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    #endregion


  }
}

