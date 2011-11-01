using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Interface
{
  public class RegGetDotTypeStackInfoResponseData : IResponseData
  {
    private IShopperContext _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
    private ISiteContext _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
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

      AtlantisException aex = new AtlantisException("DotTypeStackCacheInfo.DotTypeStackCacheInfo"
                                                    , HttpContext.Current.Request.Url.ToString()
                                                    , "0"
                                                    , ex.Message
                                                    , ex.Source
                                                    , this._shopperContext.ShopperId
                                                    , string.Empty
                                                    , HttpContext.Current.Request.UserHostAddress
                                                    , this._siteContext.Pathway
                                                    , this._siteContext.PageCount);
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
              AtlantisException aex = new AtlantisException("DotTypeStackCache.GetPriceForTld"
                                                            ,
                                                            HttpContext.Current != null
                                                                ? HttpContext.Current.Request.Url.ToString()
                                                                : null
                                                            , "0"
                                                            ,
                                                            "The promo code or tld does not exist in the stack tlds data cache"
                                                            , "DotTypeStackCache"
                                                            , this._shopperContext.ShopperId
                                                            , string.Empty
                                                            ,
                                                            HttpContext.Current != null
                                                                ? HttpContext.Current.Request.UserHostAddress
                                                                : null
                                                            , this._siteContext.Pathway
                                                            , this._siteContext.PageCount);
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
        AtlantisException aex = new AtlantisException("DotTypeStackCache.GetStackIdForTld"
                                                , HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : null
                                                , "0"
                                                , "The promo code or tld does not exist in the stack tlds datacache"
                                                , "DotTypeStackCache"
                                                , this._shopperContext.ShopperId
                                                , string.Empty
                                                , HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null
                                                , this._siteContext.Pathway
                                                , this._siteContext.PageCount);
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
