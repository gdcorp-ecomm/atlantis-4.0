using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Currency.Interface
{
  public class CurrencyTypesResponseData : IResponseData, IEnumerable<ICurrencyInfo>
  {
    public static CurrencyTypesResponseData FromCacheXml(string cacheXml)
    {
      return new CurrencyTypesResponseData(cacheXml);
    }

    private Dictionary<string, ICurrencyInfo> _currencyInfoSet;
    private List<ICurrencyInfo> _currencyInfoList;

    private CurrencyTypesResponseData(string cacheXml)
    {
      _currencyInfoList = new List<ICurrencyInfo>();
      _currencyInfoSet = new Dictionary<string, ICurrencyInfo>(StringComparer.OrdinalIgnoreCase);

      if (!string.IsNullOrEmpty(cacheXml))
      {
        try
        {
          var cacheElement = XElement.Parse(cacheXml);
          var currencyTypes = cacheElement.Descendants("currency");
          foreach (XElement currencyElement in currencyTypes)
          {
            CurrencyInfo info = CurrencyInfo.FromCacheElement(currencyElement);
            if (info != null)
            {
              _currencyInfoList.Add(info);
              _currencyInfoSet[info.CurrencyType] = info;
            }
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          AtlantisException exception = new AtlantisException("CurrencyTypesResponseData", "0", message, cacheXml, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }
      }
    }

    public int Count
    {
      get { return _currencyInfoList.Count; }
    }

    public ICurrencyInfo this[string currencyType]
    {
      get
      {
        ICurrencyInfo result = null;
        _currencyInfoSet.TryGetValue(currencyType, out result);
        return result;
      }
    }

    public bool Contains(string currencyType)
    {
      return _currencyInfoSet.ContainsKey(currencyType);
    }

    public string ToXML()
    {
      var currencydata = new XElement("currencytypes");

      foreach (ICurrencyInfo info in this)
      {
        var infoElement = new XElement("currency",
          new XAttribute("type", info.CurrencyType),
          new XAttribute("description", info.Description),
          new XAttribute("descriptionplural", info.DescriptionPlural),
          new XAttribute("symbol", info.Symbol),
          new XAttribute("symbolhtml", info.SymbolHtml),
          new XAttribute("symbolposition", info.SymbolPosition.ToString()),
          new XAttribute("decimalprecision", info.DecimalPrecision.ToString(CultureInfo.InvariantCulture)),
          new XAttribute("decimalseparator", info.DecimalSeparator),
          new XAttribute("thousandsseparator", info.ThousandsSeparator),
          new XAttribute("istransactional", info.IsTransactional.ToString()),
          new XAttribute("isactive", info.IsActive.ToString()),
          new XAttribute("exchangerate", info.ExchangeRate.ToString(CultureInfo.InvariantCulture)),
          new XAttribute("exchangeratepricing", info.ExchangeRatePricing.ToString(CultureInfo.InvariantCulture)),
          new XAttribute("exchangerateoperating", info.ExchangeRateOperating.ToString(CultureInfo.InvariantCulture)));
        currencydata.Add(infoElement);
      }

      return currencydata.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public IEnumerator<ICurrencyInfo> GetEnumerator()
    {
      return _currencyInfoList.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _currencyInfoList.GetEnumerator();
    }
  }
}
