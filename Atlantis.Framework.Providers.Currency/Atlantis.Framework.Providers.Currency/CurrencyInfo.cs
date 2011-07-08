using System;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Currency
{
  public class CurrencyInfo : ICurrencyInfo, IEquatable<ICurrencyInfo>
  {
    XElement _currencyElement;
    double _exchangeRate;
    double _exchangeRatePricing;
    double _exchangeRateOperating;
    string _symbol;
    string _symbolHtml;
    CurrencySymbolPositionType _symbolPosition;
    int _decimalPrecision;
    string _decimalSeparator;
    string _thousandsSeparator;
    string _currencyType;
    string _description;
    string _descriptionPlural;
    bool _isTransactional;

    internal CurrencyInfo(XElement currencyElement)
    {
      _currencyElement = currencyElement;

      _currencyType = LookupAttribute("gdshop_currencyType", string.Empty);
      _description = LookupAttribute("description", string.Empty);
      _descriptionPlural = LookupAttribute("description2", string.Empty);
      _exchangeRate = LookupDoubleAttribute("exchangeRate", 0);
      _exchangeRatePricing = LookupDoubleAttribute("exchangeRatePricing", 0);
      _exchangeRateOperating = LookupDoubleAttribute("exchangeRateOperating", 0);
      _symbol = LookupAttribute("currencySymbol", "$");
      _symbolHtml = LookupAttribute("currencySymbolHtml", "$");

      string position = LookupAttribute("currencySymbolPosition", "prefix");
      if (position.Equals("prefix", StringComparison.InvariantCultureIgnoreCase))
      {
        _symbolPosition = CurrencySymbolPositionType.Prefix;
      }
      else
      {
        _symbolPosition = CurrencySymbolPositionType.Suffix;
      }

      _decimalPrecision = LookupIntAttribute("decimalPrecision", 2);
      _decimalSeparator = LookupAttribute("decimalSeparator", ".");
      _thousandsSeparator = LookupAttribute("thousandsSeparator", ",");

      if (_currencyType.Equals("USD", StringComparison.InvariantCultureIgnoreCase))
      {
        _isTransactional = true;
      }
      else
      {
        string isTransactional = LookupAttribute("isTransactional", "0");
        _isTransactional = (isTransactional == "1");
      }
    }

    public string CurrencyType
    {
      get { return _currencyType; }
    }

    /// <summary>
    /// Currency description. If you need plural use DescriptionPlural; do not just
    /// add an s to this property.
    /// </summary>
    public string Description
    {
      get { return _description; }
    }

    /// <summary>
    /// Proper plural description of the currency
    /// </summary>
    public string DescriptionPlural
    {
      get { return _descriptionPlural; }
    }

    /// <summary>
    /// This is the base exchange rate. Do not use for dispaly as it does not have enough pricing bias.
    /// </summary>
    public double ExchangeRate
    {
      get { return _exchangeRate; }
    }

    /// <summary>
    /// This the the exchange rate used for all pricing conversions that are used to display converted
    /// prices to the user.
    /// </summary>
    public double ExchangeRatePricing
    {
      get { return _exchangeRatePricing; }
    }

    /// <summary>
    /// This rate is used by eComm only.
    /// </summary>
    public double ExchangeRateOperating
    {
      get { return _exchangeRateOperating; }
    }

    /// <summary>
    /// Currency Symbol.  Can be special characters.
    /// </summary>
    public string Symbol
    {
      get { return _symbol; }
    }

    /// <summary>
    /// Currency Symbol in html form where appropriate.  Example &euro; for Euros
    /// </summary>
    public string SymbolHtml
    {
      get { return _symbolHtml; }
    }

    public CurrencySymbolPositionType SymbolPosition
    {
      get { return _symbolPosition; }
    }

    public int DecimalPrecision
    {
      get { return _decimalPrecision; }
    }

    public string DecimalSeparator
    {
      get { return _decimalSeparator; }
    }

    public string ThousandsSeparator
    {
      get { return _thousandsSeparator; }
    }

    /// <summary>
    /// Is this currency enabled for its own catalog pricing and transactions.
    /// </summary>
    public bool IsTransactional
    {
      get { return _isTransactional; }
    }

    #region XmlAttribute Helpers
    
    private string LookupAttribute(string name, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = _currencyElement.Attribute(name);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    private double LookupDoubleAttribute(string name, double defaultValue)
    {
      double result = defaultValue;
      XAttribute attribute = _currencyElement.Attribute(name);
      if (attribute != null)
      {
        string resultText = attribute.Value;
        if (!double.TryParse(resultText, out result))
        {
          result = defaultValue;
        }
      }
      return result;
    }

    private int LookupIntAttribute(string name, int defaultValue)
    {
      int result = defaultValue;
      XAttribute attribute = _currencyElement.Attribute(name);
      if (attribute != null)
      {
        string resultText = attribute.Value;
        if (!int.TryParse(resultText, out result))
        {
          result = defaultValue;
        }
      }
      return result;
    }

    #endregion

    #region IEquatable<ICurrencyInfo> Members

    public bool Equals(ICurrencyInfo other)
    {
      bool result = false;
      if (other != null)
      {
        result = CurrencyType.Equals(other.CurrencyType, StringComparison.InvariantCultureIgnoreCase);
      }
      return result;
    }

    #endregion
  }
}
