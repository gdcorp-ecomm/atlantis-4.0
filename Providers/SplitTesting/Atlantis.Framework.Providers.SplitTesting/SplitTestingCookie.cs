using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Atlantis.Framework.Providers.SplitTesting
{
  internal class SplitTestingCookie
  {
    private const int MinExpirationHours = 1;
    private const int MaxExpirationHours = 168;
    private const int StdExpirationHours = 24;
    private const string CookielifeHours = "ATLANTIS_SPLITPROVIDER_COOKIELIFE_HOURS";

    readonly IProviderContainer _container;
    readonly Lazy<string> _cookieName;

    // ||  **** Kept for backwards compatibility. Delete in next version ****
    // \/
    readonly Lazy<string> _cookieNameOld;
    // /\ 
    // || **** Kept for backwards compatibility. Delete in next version ****

    readonly Lazy<ISiteContext> _siteContext;
    readonly Lazy<IShopperContext> _shopperContext;

    internal SplitTestingCookie(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => _container.Resolve<IShopperContext>());
      _cookieName = new Lazy<string>(LoadCookieName);
      _cookieNameOld = new Lazy<string>(LoadCookieNameOld);
    }

    // ||  **** Kept for backwards compatibility. Delete in next version ****
    // \/
    private string LoadCookieNameOld()
    {
      var shopperId = _shopperContext.Value.ShopperId.ToString(CultureInfo.InvariantCulture);
      if (string.IsNullOrEmpty(shopperId))
      {
        shopperId = "0";
      }

      return "SplitTesting" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture) + "_" + shopperId;
    }
    // /\ 
    // || **** Kept for backwards compatibility. Delete in next version ****

    private string LoadCookieName()
    {
      return "SplitTesting" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
    }

    private Dictionary<string, string> RequestCookieValues
    {
      get
      {
        var vals = new Dictionary<string, string>();

        // ||  **** Kept for backwards compatibility. Delete in next version ****
        // \/
        var oldCookie = HttpContext.Current.Request.Cookies[_cookieNameOld.Value];
        if (oldCookie != null)
        {
          foreach (var key in oldCookie.Values.AllKeys)
          {
            vals[key] = oldCookie.Values[key];
          }
        }
        // /\ 
        // || **** Kept for backwards compatibility. Delete in next version ****


        var cookie = HttpContext.Current.Request.Cookies[_cookieName.Value];
        if (cookie != null)
        {
          foreach (var key in cookie.Values.AllKeys)
          {
            vals[key] = cookie.Values[key];
          }
        }
        return vals;
      }
    }

    internal Dictionary<string, string> CookieValues
    {
      get
      {
        if ((HttpContext.Current != null) && (RequestCookieValues != null) && RequestCookieValues.Count > 0)
        {
          return RequestCookieValues;
        }
        return new Dictionary<string, string>(0);
      }
      set
      {
        if (HttpContext.Current != null)
        {
          HttpCookie splitValueCookie = _siteContext.Value.NewCrossDomainMemCookie(_cookieName.Value);

          foreach (var activeTest in value)
          {
            splitValueCookie.Values.Add(activeTest.Key, activeTest.Value);
          }

          splitValueCookie.Expires = CookieExpirationDate();

          HttpContext.Current.Response.Cookies.Set(splitValueCookie);
        }
      }
    }

    private DateTime CookieExpirationDate()
    {
      int expiration;
      string expirationHours = null;
      try
      {
        expirationHours = DataCache.DataCache.GetAppSetting(CookielifeHours);
      }
      catch (Exception)
      { //ignore and allow use of default 
      }

      if (int.TryParse(expirationHours, out expiration))
      {
        expiration = expiration > MaxExpirationHours ? MaxExpirationHours : expiration;
        expiration = expiration < MinExpirationHours ? MinExpirationHours : expiration;
      }
      else
      {
        expiration = StdExpirationHours;
      }
      DateTime expirationDate = DateTime.Now.AddHours(expiration);
      return expirationDate;
    }
  }
}
