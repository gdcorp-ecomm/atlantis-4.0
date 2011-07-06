using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.Providers
{
  public abstract class SiteContextProviderBase : ProviderBase, ISiteContext
  {
    #region ISiteContext Members

    public abstract int ContextId { get; }
    public abstract string StyleId { get; }
    public abstract int PrivateLabelId { get; }
    public abstract string ProgId { get; }

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

    private const string COOKIE_PAGECOUNT = "pagecount";
    private int? _pageCount;
    public int PageCount
    {
      get
      {
        if (_pageCount == null)
        {
          _pageCount = 0;
          HttpCookie pageCountCookie = HttpContext.Current.Request.Cookies[COOKIE_PAGECOUNT];
          if (pageCountCookie != null)
          {
            int pageCount;
            if (Int32.TryParse(pageCountCookie.Value, out pageCount))
              _pageCount = pageCount;
          }
        }

        return _pageCount.Value;
      }
    }

    private const string COOKIE_PATHWAY = "pathway";
    private string _pathway;
    public string Pathway
    {
      get
      {
        if (_pathway == null)
        {
          HttpCookie pathwayCookie = HttpContext.Current.Request.Cookies[COOKIE_PATHWAY];
          if (pathwayCookie != null)
            _pathway = pathwayCookie.Value ?? string.Empty;
          else
            _pathway = string.Empty;
        }

        return _pathway;
      }
    }

    private const string PARAM_CI = "ci";
    private string _ci;
    public string CI
    {
      get
      {
        if (_ci == null)
        {
          string ci = HttpContext.Current.Request[PARAM_CI] ?? string.Empty;
          ci = HttpUtility.HtmlEncode(ci.Trim());
          if (ci.Contains(","))
            ci = ci.Substring(0, ci.IndexOf(','));
          _ci = ci;
        }

        return _ci;
      }
    }

    private const string COOKIE_COMMISSIONJUNCTION = "commissionJunctionStartDate";
    private string _commissionJunctionStartDate;
    public string CommissionJunctionStartDate
    {
      get
      {
        if (_commissionJunctionStartDate == null)
        {
          _commissionJunctionStartDate = string.Empty;

          if (ContextId == ContextIds.GoDaddy)
          {
            HttpCookie cjCookie = HttpContext.Current.Request.Cookies[COOKIE_COMMISSIONJUNCTION];
            if (cjCookie != null)
            {
              _commissionJunctionStartDate = cjCookie.Value;
            }
          }
        }
        return _commissionJunctionStartDate;
      }
      set
      {
        if (ContextId == ContextIds.GoDaddy)
        {
          HttpCookie cjCookie = HttpContext.Current.Request.Cookies[COOKIE_COMMISSIONJUNCTION];
          if (cjCookie == null)
          {
            string cjcDaySetting = DataCache.DataCache.GetAppSetting("CJC_DAYS");
            int cjcDays;
            if (Int32.TryParse(cjcDaySetting, out cjcDays))
            {
              DateTime cjNow = DateTime.Now;
              cjCookie = NewCrossDomainCookie(COOKIE_COMMISSIONJUNCTION, cjNow.AddDays(cjcDays));
              _commissionJunctionStartDate = value;
              cjCookie.Value = value;
              HttpContext.Current.Response.Cookies.Add(cjCookie);
            }
            else
            {
              AtlantisException ex = new AtlantisException(
                "SalesBasePage.SetCommissionJunctionStartDate",
                HttpContext.Current.Request.Url.ToString(),
                "0",
                "CJC_DAYS AppSetting is not numeric.",
                "Setting=" + cjcDaySetting,
                string.Empty, string.Empty, HttpContext.Current.Request.UserHostAddress,
                Pathway, PageCount);
              Engine.Engine.LogAtlantisException(ex);
            }
          }
        }
      }
    }

    private const string PARAM_ISC = "isc";
    private string _isc;
    public string ISC
    {
      get
      {
        // TODO: This structure does not support marketings consistent changes
        // to ISC defaults or special sales that default an ISC code to a visitor
        // without one.  But ISC is needed by the base page for CI tracking.
        if (_isc == null)
        {
          string isc = HttpContext.Current.Request[PARAM_ISC] ?? string.Empty;
          if (isc.Contains(","))
            isc = isc.Substring(0, isc.IndexOf(','));
          if (isc.Length > 10)
            isc = isc.Substring(0, 10);
          _isc = isc;
        }

        return _isc;
      }
    }

    private const string COOKIE_CURRENCY_PREFIX = "currency";
    protected const string POTABLE_SOURCE_STR_KEY = "potableSourceStr";
    protected const string DEFAULT_CURRENCY_TYPE = "USD";
    private const string PARAM_CURRENCYTYPE = "currencyType";
    private string _currencyType;
    public string CurrencyType
    {
      get
      {
        if (_currencyType == null)
        {
          bool saveCookie = true;
          Dictionary<string, Dictionary<string, string>> currencyDataAll = DataCache.DataCache.GetCurrencyDataAll();

          string currencyType = HttpContext.Current.Request[PARAM_CURRENCYTYPE];
          if ((string.IsNullOrEmpty(currencyType) || (!currencyDataAll.ContainsKey(currencyType))))
          {
            currencyType = GetCurrencyTypeFromCookie();
            if ((!string.IsNullOrEmpty(currencyType)) && currencyDataAll.ContainsKey(currencyType))
            {
              saveCookie = false;
            }
            else
            {
              currencyType = DEFAULT_CURRENCY_TYPE;
            }
          }

          _currencyType = currencyType;
          if (saveCookie)
          {
            SaveCurrencyIntoCookie();
          }
        }

        return _currencyType;
      }
    }

    protected string CrossDomainCurrencyCookieName
    {
      get { return COOKIE_CURRENCY_PREFIX + PrivateLabelId; }
    }

    private void SaveCurrencyIntoCookie()
    {
      HttpCookie currencyCookie = HttpContext.Current.Request.Cookies[CrossDomainCurrencyCookieName];

      if (currencyCookie == null
         || !currencyCookie.HasKeys
         || currencyCookie[POTABLE_SOURCE_STR_KEY] != _currencyType)
      {
        currencyCookie = NewCrossDomainCookie(CrossDomainCurrencyCookieName, DateTime.UtcNow.Add(TimeSpan.FromDays(365)));
        currencyCookie[POTABLE_SOURCE_STR_KEY] = _currencyType;
        HttpContext.Current.Response.Cookies.Add(currencyCookie);
      }
    }

    protected string GetCurrencyTypeFromCookie()
    {
      string sResult = null;

      HttpCookie currencyCookie = HttpContext.Current.Request.Cookies[CrossDomainCurrencyCookieName];
      if (currencyCookie != null && currencyCookie.HasKeys)
        sResult = currencyCookie[POTABLE_SOURCE_STR_KEY];

      return sResult;
    }

    public virtual void SetCurrencyType(string currencyType)
    {
      Dictionary<string, Dictionary<string, string>> currencyDataAll = DataCache.DataCache.GetCurrencyDataAll();
      if (!currencyDataAll.ContainsKey(currencyType))
      {
        currencyType = DEFAULT_CURRENCY_TYPE;
      }

      _currencyType = currencyType;
      SaveCurrencyIntoCookie();
    }

    private bool? _isRequestInternal;
    public virtual bool IsRequestInternal
    {
      get
      {
        if (_isRequestInternal == null)
        {
          _isRequestInternal = RequestHelper.IsRequestInternal();
        }
        return (bool)_isRequestInternal;
      }
    }

    private ServerLocationType _serverLocation = ServerLocationType.Undetermined;
    public ServerLocationType ServerLocation
    {
      get
      {
        if (_serverLocation == ServerLocationType.Undetermined)
        {
          _serverLocation = RequestHelper.GetServerLocation(IsRequestInternal);
        }
        return _serverLocation;
      }
    }

    private IManagerContext _managerContext;
    public IManagerContext Manager
    {
      get 
      { 
        if (_managerContext == null)
        {
          _managerContext = Container.Resolve<IManagerContext>();
        }
        return _managerContext;
      }
    }

    #endregion

    protected SiteContextProviderBase(IProviderContainer providerContainer) : base(providerContainer)
    {
    }
  }
}
