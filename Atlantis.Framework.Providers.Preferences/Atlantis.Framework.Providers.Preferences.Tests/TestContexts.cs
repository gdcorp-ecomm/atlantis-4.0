using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.Preferences.Tests
{
  internal class TestContexts : ProviderBase, ISiteContext, IShopperContext, IManagerContext
  {
    int _privateLabelId = 1;
    string _shopperId = string.Empty;

    public TestContexts(IProviderContainer container)
      : base(container)
    {
    }

    public void SetContextInfo(int privateLabelId, string shopperId)
    {
      _privateLabelId = privateLabelId;
      _shopperId = shopperId;
    }

    #region ISiteContext Members

    public int ContextId
    {
      get
      {
        int result = 6;
        if (_privateLabelId == 2)
        {
          result = 5;
        }
        else if (_privateLabelId == 1)
        {
          result = 1;
        }
        else if (_privateLabelId == 1387)
        {
          result = 2;
        }
        return result;
      }
    }

    public string StyleId
    {
      get { return "0"; }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string ProgId
    {
      get { return DataCache.DataCache.GetProgID(PrivateLabelId); }
    }

    public HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      HttpCookie result = new HttpCookie(cookieName);
      result.Expires = expiration;
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    public HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      HttpCookie result = new HttpCookie(cookieName);
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    private string _crossDomainCookieDomain;
    private string CrossDomainCookieDomain
    {
      get
      {
        if (_crossDomainCookieDomain == null)
        {
          string result = HttpContext.Current.Request.Url.Host;
          if (result == "localhost")
            result = null;
          else if (result.Contains("."))
          {
            string[] parts = result.Split('.');
            if (parts.Length > 2)
              result = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
          }
          _crossDomainCookieDomain = result;
        }
        return _crossDomainCookieDomain;
      }
    }

    public int PageCount
    {
      get { return 0; }
    }

    public string Pathway
    {
      get { return "UnitTest"; }
    }

    public string CI
    {
      get { return string.Empty; }
    }

    public string CommissionJunctionStartDate
    {
      get
      {
        return string.Empty;
      }
      set
      {
        return;
      }
    }

    public string ISC
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request.QueryString["ISC"] != null)
          {
            result = HttpContext.Current.Request.QueryString["ISC"];
          }
        }
        return result;
      }
    }

    public string CurrencyType
    {
      get
      {
        return "USD";
      }
    }

    public void SetCurrencyType(string currencyType)
    {
      return;
    }

    public bool IsRequestInternal
    {
      get
      {
        return true;
      }
    }

    public ServerLocationType ServerLocation
    {
      get { return ServerLocationType.Dev; }
    }

    public IManagerContext Manager
    {
      get { return this; }
    }

    #endregion

    #region IShopperContext Members

    public int ShopperPriceType
    {
      get { return 0; }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return ShopperStatusType.Public; }
    }

    public void ClearShopper()
    {
      _shopperId = string.Empty;
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      _shopperId = shopperId;
      return true;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      _shopperId = shopperId;
      return true;
    }

    public void SetNewShopper(string shopperId)
    {
      _shopperId = shopperId;
    }

    #endregion

    #region IManagerContext Members

    public bool IsManager
    {
      get { return false; }
    }

    public string ManagerUserId
    {
      get { return string.Empty; }
    }

    public string ManagerUserName
    {
      get { return string.Empty; }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { return null; }
    }

    public string ManagerShopperId
    {
      get { return string.Empty; }
    }

    public int ManagerPrivateLabelId
    {
      get { return 0; }
    }

    public int ManagerContextId
    {
      get { return 0; }
    }

    #endregion
  }
}
