using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Atlantis.Framework.Providers.Preferences
{
  internal class PreferenceCookie
  {
    private readonly IProviderContainer _container;

    /// <summary>
    /// Once all the sites have the new preferences provider out in them,
    /// We can remove the legacy cookie code from here and have them drop in 
    /// and update.
    /// </summary>
    private const string _SHOPPERIDPREFKEY = "_sid";
    private const string _COOKIENAME = "preferences";

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<Dictionary<string,string>> _loadedCookieValues;
    private readonly Lazy<string> _legacyCookieName;

    internal PreferenceCookie(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => { return _container.Resolve<ISiteContext>(); });
      _shopperContext = new Lazy<IShopperContext>(() => { return container.Resolve<IShopperContext>(); });
      _loadedCookieValues = new Lazy<Dictionary<string,string>>(LoadCookie);
      _legacyCookieName = new Lazy<string>(DetermineLegacyCookieName);
    }

    private string DetermineLegacyCookieName()
    {
      return _COOKIENAME + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
    }

    private Dictionary<string, string> LoadCookie()
    {
      Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      if ((HttpContext.Current == null) || (HttpContext.Current.Request.Cookies == null))
      {
        return result;
      }

      var cookie = HttpContext.Current.Request.Cookies[_COOKIENAME];
      if ((cookie == null) || (!cookie.HasKeys))
      {
        cookie = HttpContext.Current.Request.Cookies[_legacyCookieName.Value];
        if ((cookie == null) || (!cookie.HasKeys))
        {
          return result;
        }
      }

      foreach (var key in cookie.Values.AllKeys)
      {
        if (key == _SHOPPERIDPREFKEY)
        {
          continue;
        }

        string[] values = cookie.Values.GetValues(key);
        if ((values != null) && (values.Length > 0))
        {
          string loadAsKey = TranslateFromLegacy(key);
          result[loadAsKey] = values[0];
        }
      }

      return result;
    }

    public bool HasPreference(string preferenceKey)
    {
      preferenceKey = TranslateFromLegacy(preferenceKey);
      return _loadedCookieValues.Value.ContainsKey(preferenceKey);
    }

    public string GetPreference(string preferenceKey)
    {
      preferenceKey = TranslateFromLegacy(preferenceKey);
      return _loadedCookieValues.Value[preferenceKey];
    }

    public void UpdatePreference(string preferenceKey, string value)
    {
      preferenceKey = TranslateFromLegacy(preferenceKey);

      string oldValue = null;
      if (_loadedCookieValues.Value.ContainsKey(preferenceKey))
      {
        oldValue = _loadedCookieValues.Value[preferenceKey];
      }

      if (oldValue != value)
      {
        _loadedCookieValues.Value[preferenceKey] = value;
        WriteCookie();
      }
    }

    public void WriteCookie()
    {
      if ((HttpContext.Current == null) || (HttpContext.Current.Request.Cookies == null))
      {
        return;
      }

      HttpCookie preferenceCookie = _siteContext.Value.NewCrossDomainMemCookie(_COOKIENAME);
      HttpCookie legacyCookie = _siteContext.Value.NewCrossDomainMemCookie(_legacyCookieName.Value);

      legacyCookie.Values[_SHOPPERIDPREFKEY] = GetShopperIdForLegcacyCookie();

      foreach (var key in _loadedCookieValues.Value.Keys)
      {
        preferenceCookie.Values[key] = _loadedCookieValues.Value[key];
        legacyCookie.Values[TranslateToLegacy(key)] = _loadedCookieValues.Value[key];
      }

      HttpContext.Current.Response.Cookies.Set(preferenceCookie);
      HttpContext.Current.Response.Cookies.Set(legacyCookie);
    }

    private string GetShopperIdForLegcacyCookie()
    {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        using (var cookieEncryption = CookieEncryption.CreateDisposable())
        {
          result = cookieEncryption.EncryptCookieValue(_shopperContext.Value.ShopperId);
        }
      }

      return result;
    }

    private string TranslateFromLegacy(string legacyKey)
    {
      string result = legacyKey;

      if (result == "gdshop_currencyType")
      {
        result = "currency";
      }

      return result;
    }

    private string TranslateToLegacy(string newKey)
    {
      string result = newKey;

      if (result == "currency")
      {
        result = "gdshop_currencyType";
      }

      return result;
    }

  }
}
