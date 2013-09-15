using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Globalization;

namespace Atlantis.Framework.Providers.Currency
{
  internal class CurrencyFormatting
  {
    private readonly IProviderContainer _container;
    private readonly Lazy<ILocalizationProvider> _localizationProvider;

    internal CurrencyFormatting(IProviderContainer container)
    {
      _container = container;
      _localizationProvider = new Lazy<ILocalizationProvider>(LoadLocalizationProvider);
    }

    private ILocalizationProvider LoadLocalizationProvider()
    {
      ILocalizationProvider result;
      if (!_container.TryResolve(out result))
      {
        result = null;
      }
      return result;
    }

    public string FormatPrice(ICurrencyPrice price, PriceFormatOptions options)
    {
      double priceToFormat = ToDouble(price);
      NumberFormatInfo formatter = GetFormatter(price.CurrencyInfo, options);
      return priceToFormat.ToString("c", formatter);
    }

    private NumberFormatInfo GetFormatter(ICurrencyInfo currencyInfo, PriceFormatOptions options)
    {
      NumberFormatInfo result;

      if (_localizationProvider.Value != null)
      {
        result = (NumberFormatInfo)_localizationProvider.Value.CurrentCultureInfo.NumberFormat.Clone();
      }
      else
      {
        result = new NumberFormatInfo();
        if (currencyInfo.SymbolPosition == CurrencySymbolPositionType.Prefix)
        {
          result.CurrencyNegativePattern = 1;
          result.CurrencyPositivePattern = 0;
        }
        else
        {
          result.CurrencyNegativePattern = 5;
          result.CurrencyPositivePattern = 1;
        }
      }

      SetNegativeFormat(result, options);
      SetSymbol(result, options, currencyInfo);
      SetPrecision(result, options, currencyInfo);

      return result;
    }

    private double ToDouble(ICurrencyPrice price)
    {
      double result;

      if (price.CurrencyInfo.DecimalPrecision > 0)
      {
        double divideBy = Math.Pow(10, price.CurrencyInfo.DecimalPrecision);
        result = price.Price / divideBy;
      }
      else
      {
        result = price.Price;
      }

      return result;
    }

    private void SetNegativeFormat(NumberFormatInfo formatter, PriceFormatOptions options)
    {
      if (options.HasFlag(PriceFormatOptions.NegativeParentheses))
      {
        switch (formatter.CurrencyNegativePattern)
        {
          case 1:
          case 2:
          case 3:
            formatter.CurrencyNegativePattern = 0;
            break;
          case 5:
          case 6:
          case 7:
            formatter.CurrencyNegativePattern = 4;
            break;
          case 9:
          case 11:
          case 12:
            formatter.CurrencyNegativePattern = 14;
            break;
          case 8:
          case 10:
          case 13:
            formatter.CurrencyNegativePattern = 15;
            break;
        }
      }
      else
      {
        switch (formatter.CurrencyNegativePattern)
        {
          case 0:
            formatter.CurrencyNegativePattern = 1;
            break;
          case 4:
            formatter.CurrencyNegativePattern = 5;
            break;
          case 14:
            formatter.CurrencyNegativePattern = 9;
            break;
          case 15:
            formatter.CurrencyNegativePattern = 8;
            break;
        }
      }
    }

    private void SetSymbol(NumberFormatInfo formatter, PriceFormatOptions options, ICurrencyInfo currencyInfo)
    {
      if (options.HasFlag(PriceFormatOptions.DropSymbol))
      {
        formatter.CurrencySymbol = string.Empty;
      }
      else if (options.HasFlag(PriceFormatOptions.AsciiSymbol))
      {
        formatter.CurrencySymbol = currencyInfo.Symbol;
      }
      else
      {
        formatter.CurrencySymbol = currencyInfo.SymbolHtml;
      }
    }

    private void SetPrecision(NumberFormatInfo result, PriceFormatOptions options, ICurrencyInfo currencyInfo)
    {
      if (options.HasFlag(PriceFormatOptions.DropDecimal))
      {
        result.CurrencyDecimalDigits = 0;
      }
      else
      {
        result.CurrencyDecimalDigits = currencyInfo.DecimalPrecision;
      }
    }



  }
}
