using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Interface
{
  public class RegGetDotTypeStackInfoResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _stackXML = string.Empty;
    private Dictionary<string, Dictionary<string, DotTypeStackItem>> _dotTypeStackItems = new Dictionary<string, Dictionary<string, DotTypeStackItem>>();

    public RegGetDotTypeStackInfoResponseData(Dictionary<string, Dictionary<string, DotTypeStackItem>> dotTypeStackInfo, string stackXML)
    {
      _dotTypeStackItems = dotTypeStackInfo;
      _stackXML = stackXML;
    }

    public RegGetDotTypeStackInfoResponseData(Dictionary<string, Dictionary<string, DotTypeStackItem>> dotTypeStackInfo, string stackXML, AtlantisException exAtlantis)
    {
      _dotTypeStackItems = dotTypeStackInfo;
      _stackXML = stackXML;
      _exception = exAtlantis;
    }

    public RegGetDotTypeStackInfoResponseData(Dictionary<string, Dictionary<string, DotTypeStackItem>> dotTypeStackInfo, string stackXML, Exception ex)
    {
      _dotTypeStackItems = dotTypeStackInfo;
      _stackXML = stackXML;

      AtlantisException aex = new AtlantisException("DotTypeStackCacheInfo.DotTypeStackCacheInfo", "0", ex.Message + ex.StackTrace, stackXML, null, null);
      Engine.Engine.LogAtlantisException(aex);
    }

    public int GetPriceForTld(string tld, string promoCode)
    {
      return GetPriceForTld(tld, promoCode, true);
    }

    public int GetPriceForTld(string tld, string promoCode, bool logExceptionOnError)
    {
      int price = 0;
      if (_dotTypeStackItems.ContainsKey(promoCode) && _dotTypeStackItems[promoCode].ContainsKey(tld))
      {
        price = _dotTypeStackItems[promoCode][tld].Price;
      }
      else
      {
        if (logExceptionOnError)
        {
          AtlantisException aex = new AtlantisException("DotTypeStackCache.GetPriceForTld", "0", "The promo code or tld does not exist in the stack tlds data cache", _stackXML, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      return price;
    }

    public int GetStackIdForTld(string tld, string promoCode)
    {
      int stackId = 0;
      if (_dotTypeStackItems.ContainsKey(promoCode) && _dotTypeStackItems[promoCode].ContainsKey(tld))
      {
        stackId = _dotTypeStackItems[promoCode][tld].StackId;
      }
      else
      {
        AtlantisException aex = new AtlantisException("DotTypeStackCache.GetStackIdForTld", "0", "The promo code or tld does not exist in the stack tlds data cache", _stackXML, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
      return stackId;
    }

    public int Count
    {
      get
      {
        return _dotTypeStackItems.Count;
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _stackXML;
    }

    #endregion
  }
}
