using System;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Products
{
  public class ProductView : IProductView
  {
    private int _quantity = 1;
    private IProduct _product;
    private bool _isDefault;
    private ICurrencyPrice _viewPrice;
    private int _savingsPercentage;
    private bool _isDiscounted;
    private string _labelText;
    private PriceRoundingType _priceRoundingMethod = PriceRoundingType.RoundFractionsUpProperly;
    private SavingsRoundingType _savingsRoundingMethod = SavingsRoundingType.FloorSavingsProperly;
    private ICurrencyProvider _currencyProvider;

    public IProduct Product
    {
      get { return _product; }
    }

    public PriceRoundingType PriceRoundingMethod
    {
      get { return _priceRoundingMethod; }
      set { _priceRoundingMethod = value; }
    }

    public SavingsRoundingType SavingsRounding
    {
      get { return _savingsRoundingMethod; }
      set { _savingsRoundingMethod = value; }
    }

    public bool IsDefault
    {
      get { return _isDefault; }
      set { _isDefault = value; }
    }

    internal ProductView(ICurrencyProvider currencyProvider, IProduct product, bool isDefault, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      _currencyProvider = currencyProvider;
      _product = product;
      IsDefault = isDefault;
      _priceRoundingMethod = priceRoundingMethod;
      _savingsRoundingMethod = savingsRoundingMethod;
    }

    internal ProductView(ICurrencyProvider currencyProvider, IProduct product, bool isDefault, int quantity, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod)
    {
      _currencyProvider = currencyProvider;
      _product = product;
      IsDefault = isDefault;
      _priceRoundingMethod = priceRoundingMethod;
      _savingsRoundingMethod = savingsRoundingMethod;

      if (quantity < 1)
      {
        quantity = 1;
      }
      _quantity = quantity;
    }

    public string LabelText
    {
      get { return _labelText; }
      set { _labelText = value; }
    }

    public bool IsDiscounted
    {
      get { return _isDiscounted; }
    }

    public int SavingsPercentage
    {
      get { return _savingsPercentage; }
    }

    public int Quantity
    {
      get
      {
        return _quantity;
      }
    }

    #region Savings and Discounts

    private ICurrencyPrice GetBaseEquivalentListPrice(IProduct baseProduct)
    {
      ICurrencyPrice result = baseProduct.ListPrice;
      if ((_product.DurationUnit != baseProduct.DurationUnit) || (_product.Duration != baseProduct.Duration))
      {
        double factor;
        if (_product.DurationUnit == RecurringPaymentUnitType.Annual)
        {
          factor = _product.Years / baseProduct.Years;
        }
        else
        {
          factor = (double)_product.Months / baseProduct.Months;
        }

        int equivalentPrice = (int)Math.Floor(baseProduct.ListPrice.Price * factor);
        result = _currencyProvider.NewCurrencyPrice(equivalentPrice, baseProduct.ListPrice.CurrencyInfo, baseProduct.ListPrice.Type);
      }
      return result;
    }

    private ICurrencyPrice GetBaseEquivalentCurrentPrice(IProduct baseProduct)
    {
      ICurrencyPrice result = baseProduct.CurrentPrice;

      if ((_product.DurationUnit != baseProduct.DurationUnit) || (_product.Duration != baseProduct.Duration))
      {
        double factor;
        if (_product.DurationUnit == RecurringPaymentUnitType.Annual)
        {
          factor = _product.Years / baseProduct.Years;
        }
        else
        {
          factor = (double)_product.Months / baseProduct.Months;
        }

        int equivalentPrice = (int)Math.Floor(baseProduct.CurrentPrice.Price * factor);
        result = _currencyProvider.NewCurrencyPrice(equivalentPrice, baseProduct.CurrentPrice.CurrencyInfo, baseProduct.CurrentPrice.Type);
      }

      return result;
    }

    private void CalculateSavings(IProduct baseProduct, out ICurrencyPrice baseEquivalentListPrice)
    {
      _savingsPercentage = 0;
      baseEquivalentListPrice = GetBaseEquivalentListPrice(baseProduct);
      if (baseProduct.ListPrice != UnitPrice)
      {
        _savingsPercentage = GetSavings(baseEquivalentListPrice);
      }
    }

    private ICurrencyPrice GetPeriodPrice(ICurrencyPrice price, double periods)
    {
      int resultPrice = price.Price;

      if(periods > 0)
      {
        if (_priceRoundingMethod == PriceRoundingType.RoundFractionsUpProperly)
        {
          resultPrice = (Int32)Math.Ceiling(price.Price / periods);
        }
        else
        {
          resultPrice = (Int32)Math.Floor(price.Price / periods);
        }
      }

      return _currencyProvider.NewCurrencyPrice(resultPrice, price.CurrencyInfo, price.Type);
    }

    public void CalculateSavings(IProduct baseProduct)
    {
      ICurrencyPrice baseEquivalentListPrice;
      CalculateSavings(baseProduct, out baseEquivalentListPrice);
    }

    public void CalculateSavingsBasedOnCurrentPriceOnly(IProduct baseProduct)
    {
      _savingsPercentage = 0;
      if (baseProduct.CurrentPrice != UnitPrice)
      {
        ICurrencyPrice baseEquivalentPromoPrice = GetBaseEquivalentCurrentPrice(baseProduct);
        _savingsPercentage = GetSavings(baseEquivalentPromoPrice);
      }
    }

    private int GetSavings(ICurrencyPrice basePrice)
    {
      int savingsPercentage = 0;
      double savings = Math.Max((basePrice.Price - UnitPrice.Price), 0);
      if (savings > 0)
      {
        if (SavingsRounding == SavingsRoundingType.FloorSavingsProperly)
        {
          savingsPercentage = (int)(Math.Floor((savings / basePrice.Price) * 100.0));
        }
        else
        {
          savingsPercentage = (int)(Math.Round((savings / basePrice.Price) * 100.0));
        }
      }
      return savingsPercentage;
    }

    public void CalculateSavingsWithMinimumSavingsPercent(IProduct baseProduct, int minimumSavingsPercent)
    {
      ICurrencyPrice baseListPrice;
      CalculateSavings(baseProduct, out baseListPrice);
      if (_savingsPercentage < minimumSavingsPercent)
      {
        _isDiscounted = true;
        double discountFactor = (100.0 - minimumSavingsPercent) / 100.0;
        if (SavingsRounding == SavingsRoundingType.FloorSavingsProperly)
        {
          _viewPrice = _currencyProvider.NewCurrencyPrice((int)(Math.Ceiling(baseListPrice.Price * discountFactor)), baseListPrice.CurrencyInfo, baseListPrice.Type);
        }
        else
        {
          _viewPrice = _currencyProvider.NewCurrencyPrice((int)(Math.Round(baseListPrice.Price * discountFactor)), baseListPrice.CurrencyInfo, baseListPrice.Type);
        }
        _savingsPercentage = minimumSavingsPercent;
      }
    }

    public void CalculateSavingsWithAddtionalSavingsPercent(IProduct baseProduct, int additionalSavingsPercent)
    {
      ICurrencyPrice baseListPrice;
      CalculateSavings(baseProduct, out baseListPrice);
      if (additionalSavingsPercent > 0)
      {
        _isDiscounted = true;
        _savingsPercentage = _savingsPercentage + additionalSavingsPercent;
        double discountFactor = (100.0 - (_savingsPercentage)) / 100.0;
        if (SavingsRounding == SavingsRoundingType.FloorSavingsProperly)
        {
          _viewPrice = _currencyProvider.NewCurrencyPrice((int)(Math.Ceiling(baseListPrice.Price * discountFactor)), baseListPrice.CurrencyInfo, baseListPrice.Type);
        }
        else
        {
          _viewPrice = _currencyProvider.NewCurrencyPrice((int)(Math.Round(baseListPrice.Price * discountFactor)), baseListPrice.CurrencyInfo, baseListPrice.Type);
        }
      }
    }

    public void CalculateSavingsWithFixedDiscountPrice(IProduct baseProduct, ICurrencyPrice fixedDiscountPrice)
    {
      if (fixedDiscountPrice.Price > 0)
      {
        _isDiscounted = true;
        _viewPrice = _currencyProvider.NewCurrencyPrice(Math.Max(_product.CurrentPrice.Price - fixedDiscountPrice.Price, 0), 
                                       _product.CurrentPrice.CurrencyInfo, 
                                       _product.CurrentPrice.Type);

        CalculateSavings(baseProduct);
      }
    }

    public void CalculateSavingsWithFixedSalePrice(IProduct baseProduct, ICurrencyPrice fixedSalePrice)
    {
      if ((fixedSalePrice.Price < UnitPrice.Price) && (fixedSalePrice.Price >= 0))
      {
        _isDiscounted = true;
        _viewPrice = fixedSalePrice;
        CalculateSavings(baseProduct);
      }
    }

    #endregion

    #region IProductView Members

    public ICurrencyPrice MonthlyListPrice
    {
      get
      {
        ICurrencyPrice result;
        if (Product.Months == 1)
        {
          result = Product.ListPrice;
        }
        else
        {
          result = GetPeriodPrice(Product.ListPrice, Product.Months);
        }

        return result;
      }
    }

    public ICurrencyPrice MonthlyCurrentPrice
    {
      get
      {
        ICurrencyPrice result;
        if (Product.Months == 1)
        {
          result = UnitPrice;
        }
        else
        {
          result = GetPeriodPrice(UnitPrice, Product.Months);
        }

        return result;
      }
    }

    public ICurrencyPrice YearlyListPrice
    {
      get { return GetPeriodPrice(Product.ListPrice, Product.Years); }
    }

    public ICurrencyPrice YearlyCurrentPrice
    {
      get
      {
        ICurrencyPrice result;
        if (Product.Years == 1)
        {
          result = UnitPrice;
        }
        else
        {
          result = GetPeriodPrice(UnitPrice, Product.Years);
        }

        return result;
      }
    }

    public ICurrencyPrice TotalPrice
    {
      get
      {
        ICurrencyPrice result;
        if (Quantity > 1)
        {
          result = _currencyProvider.NewCurrencyPrice(UnitPrice.Price * Quantity, UnitPrice.CurrencyInfo, UnitPrice.Type);
        }
        else
        {
          result = UnitPrice;
        }

        return result;
      }
    }

    public ICurrencyPrice UnitPrice
    {
      get
      {
        ICurrencyPrice result;
        if (_viewPrice == null)
        {
          if (_quantity == 1)
          {
            result = _product.CurrentPrice;
          }
          else
          {
            result = _product.GetCurrentPriceByQuantity(_quantity);
          }
        }
        else
        {
          result = _viewPrice;
        }
        return result;
      }
    }

    #endregion

    public override string ToString()
    {
      return _product.ProductId + ": " + _product.Info.Name;
    }
  }
}
