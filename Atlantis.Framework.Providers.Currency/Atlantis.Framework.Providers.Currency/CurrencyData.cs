using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Currency
{
  public class CurrencyData : IEnumerable<ICurrencyInfo>
  {
    List<ICurrencyInfo> _currencyInfoItemList;
    Dictionary<string, ICurrencyInfo> _currencyInfoItemSet;

    internal CurrencyData(XElement currencyInfoXml)
    {
      _currencyInfoItemList = new List<ICurrencyInfo>();
      _currencyInfoItemSet = new Dictionary<string, ICurrencyInfo>(StringComparer.InvariantCultureIgnoreCase);

      IEnumerable<XElement> currencyNodes = currencyInfoXml.Elements("currency");
      foreach (XElement currencyNode in currencyNodes)
      {
        ICurrencyInfo currencyInfoItem = new CurrencyInfo(currencyNode);
        _currencyInfoItemList.Add(currencyInfoItem);
        _currencyInfoItemSet[currencyInfoItem.CurrencyType] = currencyInfoItem;
      }
    }

    public ICurrencyInfo this[string currencyType]
    {
      get 
      {
        ICurrencyInfo result = null;
        _currencyInfoItemSet.TryGetValue(currencyType, out result); 
        return result;
      }
    }

    #region IEnumerable<ICurrencyInfo> Members

    public IEnumerator<ICurrencyInfo>  GetEnumerator()
    {
 	    return _currencyInfoItemList.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
 	    return _currencyInfoItemList.GetEnumerator();
    }

    #endregion

    #region Static Methods

    internal static CurrencyData GetCurrencyData(string key)
    {
      string currencyDataXml = DataCache.DataCache.GetCurrencyDataXml(key);
      XElement currencyDataElement = XElement.Parse(currencyDataXml);
      CurrencyData result = new CurrencyData(currencyDataElement);
      return result;
    }

    private static CurrencyData AllCurrency
    {
      get
      {
        return DataCache.DataCache.GetCustomCacheData<CurrencyData>("{all}", GetCurrencyData);
      }
    }

    public static IEnumerable<ICurrencyInfo> CurrencyInfoList
    {
      get { return AllCurrency; }
    }

    public static ICurrencyInfo GetCurrencyInfo(string currencyType)
    {
      return AllCurrency[currencyType];
    }

    #endregion
  }
}
