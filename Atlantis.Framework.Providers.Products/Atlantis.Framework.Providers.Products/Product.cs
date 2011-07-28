using System;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Products
{
  public class Product : IProduct
  {
    private IProductInfo _productInfo;
    private int _productId;
    private int _privateLabelId;
    private ICurrencyProvider _currencyProvider;

    internal Product(int productId, int privateLabelId, ICurrencyProvider currencyProvider)
    {
      _productId = productId;
      _privateLabelId = privateLabelId;
      _currencyProvider = currencyProvider;
      _productInfo = new ProductInfo(productId, _privateLabelId);
    }

    public IProductInfo Info
    {
      get { return _productInfo; }
    }

    public int ProductId
    {
      get { return _productId; }
    }

    public double Years
    {
      get
      {
        double years;

        switch (DurationUnit)
        {
          case RecurringPaymentUnitType.Monthly:
            years = Duration / 12.0;
            break;
          case RecurringPaymentUnitType.Annual:
            years = Duration;
            break;
          case RecurringPaymentUnitType.SemiAnnual:
            years = Duration / 2.0;
            break;
          case RecurringPaymentUnitType.Quarterly:
            years = Duration / 4.0;
            break;
          default:
            string message = "Product DurationUnit Error";
            string data = string.Concat("Years:ProductId=", ProductId.ToString(), ":DurationUnit=", DurationUnit.ToString());
            AtlantisException ex = new AtlantisException("Product.Years", "30", message, data, null, null);
            Engine.Engine.LogAtlantisException(ex);
            years = Duration;
            break;
        }

        return years;
      }

    }

    public int Months
    {
      get
      {
        int duration;
        
        switch(DurationUnit)
        {
          case RecurringPaymentUnitType.Monthly:
            duration = Duration;
            break;
          case RecurringPaymentUnitType.Annual:
            duration = Duration * 12;
            break;
          case RecurringPaymentUnitType.SemiAnnual:
            duration = Duration * 6;
            break;
          case RecurringPaymentUnitType.Quarterly:
            duration = Duration * 3;
            break;
          default:
            string message = "Product DurationUnit Error";
            string data = string.Concat("Months:ProductId=", ProductId.ToString(), ":DurationUnit=", DurationUnit.ToString());
            AtlantisException ex = new AtlantisException("Product.Months", "30", message, data, null, null);
            Engine.Engine.LogAtlantisException(ex);
            duration = Duration;
            break;
        }

        return duration;
      }
    }

    public bool IsOnSale
    {
      get
      {
        return _currencyProvider.IsProductOnSale(ProductId);
      }
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

    public ICurrencyPrice GetCurrentPriceByQuantity(int quantity)
    {
      return _currencyProvider.GetCurrentPriceByQuantity(ProductId, quantity);
    }
  }
}

