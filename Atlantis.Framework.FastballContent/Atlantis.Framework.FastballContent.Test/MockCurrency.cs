using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballContent.Test
{
  internal class MockCurrency : ProviderBase, ICurrencyProvider
  {
    public MockCurrency(IProviderContainer container)
      : base(container)
    {

    }

    public ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo, CurrencyConversionRoundingType conversionRoundingType = CurrencyConversionRoundingType.Round)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<ICurrencyInfo> CurrencyInfoList
    {
      get { throw new NotImplementedException(); }
    }

    public ICurrencyInfo GetCurrencyInfo(string currencyType)
    {
      throw new NotImplementedException();
    }

    public ICurrencyPrice GetCurrentPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
    {
      throw new NotImplementedException();
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null)
    {
      throw new NotImplementedException();
    }

    public ICurrencyPrice GetListPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null)
    {
      throw new NotImplementedException();
    }

    public ICurrencyInfo GetValidCurrencyInfo(string currencyType)
    {
      throw new NotImplementedException();
    }

    public bool IsCurrencyTransactionalForContext(ICurrencyInfo currencyToCheck)
    {
      throw new NotImplementedException();
    }

    public bool IsProductOnSale(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
    {
      throw new NotImplementedException();
    }

    public ICurrencyPrice NewCurrencyPrice(int price, ICurrencyInfo currencyInfo, CurrencyPriceType currencyPriceType)
    {
      throw new NotImplementedException();
    }

    public ICurrencyPrice NewCurrencyPriceFromUSD(int usdPrice, ICurrencyInfo currencyInfo = null, CurrencyConversionRoundingType conversionRoundingType = CurrencyConversionRoundingType.Round)
    {
      throw new NotImplementedException();
    }

    public string PriceFormat(ICurrencyPrice price, PriceFormatOptions priceOptions = PriceFormatOptions.None)
    {
      throw new NotImplementedException();
    }

    public string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      throw new NotImplementedException();
    }

    public string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, PriceTextOptions textOptions = PriceTextOptions.None, PriceFormatOptions formatOptions = PriceFormatOptions.None)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, CurrencyNegativeFormat negativeFormat)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal)
    {
      throw new NotImplementedException();
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices)
    {
      throw new NotImplementedException();
    }

    public ICurrencyInfo SelectedDisplayCurrencyInfo
    {
      get { throw new NotImplementedException(); }
    }

    public string SelectedDisplayCurrencyType
    {
      get
      {
        return "USD";
      }
      set
      {
        return;
      }
    }

    public ICurrencyInfo SelectedTransactionalCurrencyInfo
    {
      get { throw new NotImplementedException(); }
    }

    public string SelectedTransactionalCurrencyType
    {
      get { return "USD"; }
    }
  }
}
