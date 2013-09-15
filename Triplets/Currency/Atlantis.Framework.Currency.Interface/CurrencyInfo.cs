using System;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Currency.Interface
{
  public class CurrencyInfo : ICurrencyInfo
  {
    internal static CurrencyInfo FromCacheElement(XElement element)
    {
      CurrencyInfo result = null;

      if (element != null)
      {
        var rawCurrencyInfo = new CurrencyInfo();

        rawCurrencyInfo.CurrencyType = element.LookupAttribute("gdshop_currencyType", string.Empty);

        if (!string.IsNullOrEmpty(rawCurrencyInfo.CurrencyType))
        {
          rawCurrencyInfo.Description = element.LookupAttribute("description", string.Empty);
          rawCurrencyInfo.DescriptionPlural = element.LookupAttribute("description2", string.Empty);
          rawCurrencyInfo.ExchangeRate = element.LookupDoubleAttribute("exchangeRate", 0);
          rawCurrencyInfo.ExchangeRatePricing = element.LookupDoubleAttribute("exchangeRatePricing", 0);
          rawCurrencyInfo.ExchangeRateOperating = element.LookupDoubleAttribute("exchangeRateOperating", 0);
          rawCurrencyInfo.Symbol = element.TrimAttribute("currencySymbol", "$");
          rawCurrencyInfo.SymbolHtml = element.TrimAttribute("currencySymbolHtml", "$");

          var position = element.LookupAttribute("currencySymbolPosition", "prefix");
          if (position.Equals("prefix", StringComparison.OrdinalIgnoreCase))
          {
            rawCurrencyInfo.SymbolPosition = CurrencySymbolPositionType.Prefix;
          }
          else
          {
            rawCurrencyInfo.SymbolPosition = CurrencySymbolPositionType.Suffix;
          }

          rawCurrencyInfo.DecimalPrecision = element.LookupIntAttribute("decimalPrecision", 2);
          rawCurrencyInfo.DecimalSeparator = element.LookupAttribute("decimalSeparator", ".");
          rawCurrencyInfo.ThousandsSeparator = element.LookupAttribute("thousandsSeparator", ",");

          if (rawCurrencyInfo.CurrencyType.Equals("USD", StringComparison.OrdinalIgnoreCase))
          {
            rawCurrencyInfo.IsTransactional = true;
          }
          else
          {
            var isTransactional = element.LookupAttribute("isTransactional", "0");
            rawCurrencyInfo.IsTransactional = (isTransactional == "1");
          }

          var isActive = element.LookupAttribute("isActive", "1");
          rawCurrencyInfo.IsActive = (isActive == "1");

          result = rawCurrencyInfo;
        }
      }

      return result;
    }

    private CurrencyInfo()
    {
    }

    public string CurrencyType { get; private set; }
    public int DecimalPrecision { get; private set; }
    public string DecimalSeparator { get; private set; }
    public string Description { get; private set; }
    public string DescriptionPlural { get; private set; }
    public double ExchangeRate { get; private set; }
    public double ExchangeRatePricing { get; private set; }
    public double ExchangeRateOperating { get; private set; }
    public string Symbol { get; private set; }
    public string SymbolHtml { get; private set; }
    public CurrencySymbolPositionType SymbolPosition { get; private set; }
    public string ThousandsSeparator { get; private set; }
    public bool IsTransactional { get; private set; }
    public bool IsActive { get; private set; }

    public bool Equals(ICurrencyInfo other)
    {
      bool result = false;

      if (other != null)
      {
        result = other.CurrencyType.Equals(this.CurrencyType, StringComparison.OrdinalIgnoreCase);
      }

      return result;
    }

  }
}
