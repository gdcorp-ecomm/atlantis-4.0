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
