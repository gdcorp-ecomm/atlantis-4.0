using System;
using System.Globalization;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Products
{
  public class Product : IProduct
  {
    private readonly int _productId;
    private readonly IProviderContainer _container;
    private readonly Lazy<Dictionary<RecurringPaymentUnitType, double>> _durationUnits;
    private readonly Lazy<IProductInfo> _productInfo;
    private readonly Lazy<ICurrencyProvider> _currency; 
   
    internal Product(int productId, IProviderContainer container)
    {
      _productId = productId;
      _container = container;

      _productInfo = new Lazy<IProductInfo>(LoadProductInfo);
      _durationUnits = new Lazy<Dictionary<RecurringPaymentUnitType, double>>(CalculateDurationUnits);
      _currency = new Lazy<ICurrencyProvider>(() => _container.Resolve<ICurrencyProvider>());
    }

    private IProductInfo LoadProductInfo()
    {
      return new ProductInfo(_container, _productId);
    }

    public IProductInfo Info
    {
      get { return _productInfo.Value; }
    }

    public int ProductId
    {
      get { return _productId; }
    }

    #region Duration Units

    private Dictionary<RecurringPaymentUnitType, double> CalculateDurationUnits()
    {
      var result = new Dictionary<RecurringPaymentUnitType, double>();

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
          const string message = "Product DurationUnit Error";
          string data = string.Concat("ProductId=", ProductId.ToString(CultureInfo.InvariantCulture), ":DurationUnit=", DurationUnit.ToString());
          var ex = new AtlantisException("Product.CalculateDurationUnits", 30, message, data);
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
        return _productInfo.Value.NumberOfPeriods;
      }
    }

    public RecurringPaymentUnitType DurationUnit
    {
      get
      {
        return _productInfo.Value.RecurringPayment;
      }
    }

    #endregion

    #region OnSale

    public bool IsOnSale
    {
      get { return _currency.Value.IsProductOnSale(ProductId); }
    }

    public bool GetIsOnSale(int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
    {
      return _currency.Value.IsProductOnSale(ProductId, shopperPriceType, transactionCurrency, isc);
    }

    #endregion

    #region Pricing

    private static int ConvertToInt32NoOverflow(double value)
    {
      if (value >= int.MaxValue)
      {
        return int.MaxValue;
      }

      if (value <= int.MinValue)
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

        if ((periods > 0) && (Math.Abs(periods - 1.0) > 0))
        {
          int periodPrice = ConvertToInt32NoOverflow(roundingType == PriceRoundingType.RoundFractionsUpProperly ? Math.Ceiling(price.Price / periods) : Math.Floor(price.Price / periods));
          result = _currency.Value.NewCurrencyPrice(periodPrice, price.CurrencyInfo, price.Type);
        }
      }

      return result;
    }

    private ICurrencyPrice _listPrice;
    public ICurrencyPrice ListPrice
    {
      get { return _listPrice ?? (_listPrice = _currency.Value.GetListPrice(ProductId)); }
    }

    public ICurrencyPrice GetListPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      var price = _currency.Value.GetListPrice(ProductId, shopperPriceType, transactionCurrency);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    private ICurrencyPrice _currentPrice;
    public ICurrencyPrice CurrentPrice
    {
      get { return _currentPrice ?? (_currentPrice = _currency.Value.GetCurrentPrice(ProductId)); }
    }

    public ICurrencyPrice GetCurrentPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      var price = _currency.Value.GetCurrentPrice(ProductId, shopperPriceType, transactionCurrency, isc);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int quantity)
    {
      return _currency.Value.GetCurrentPriceByQuantity(ProductId, quantity);
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int quantity, RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly)
    {
      var price = _currency.Value.GetCurrentPriceByQuantity(ProductId, quantity, shopperPriceType, transactionCurrency);
      return GetPeriodPrice(price, durationUnit, roundingType);
    }

    #endregion


  }
}

