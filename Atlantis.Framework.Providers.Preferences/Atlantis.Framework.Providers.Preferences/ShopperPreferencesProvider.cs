using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Preferences.Cookies;
using Atlantis.Framework.ShopperPrefGet.Interface;
using Atlantis.Framework.ShopperPrefUpdate.Interface;

namespace Atlantis.Framework.Providers.Preferences
{
  public class ShopperPreferencesProvider : ProviderBase, IShopperPreferencesProvider
  {
    private const string _DBSTATUSSETTING = "ATLANTIS_PREFERENCES_DATABASE";
    private const string _SHOPPERIDPREFKEY = "_sid";
    protected const string _LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY = "potableSourceStr";

    #region Static Properties

    private static bool _useCookies = true;
    public static bool UseLegacyCookies
    {
      get { return _useCookies; }
      set { _useCookies = value; }
    }

    #endregion

    public ShopperPreferencesProvider(IProviderContainer container)
      : base(container)
    {
    }

    #region Private Properties

    private bool? _isDatabaseEnabled = null;
    private bool IsDatabaseEnabled
    {
      get
      {
        if (!_isDatabaseEnabled.HasValue)
        {
          string dbstatus = DataCache.DataCache.GetAppSetting(_DBSTATUSSETTING);
          _isDatabaseEnabled = !dbstatus.Equals("OFF");
        }
        return _isDatabaseEnabled.Value;
      }
      set
      {
        _isDatabaseEnabled = value;
      }
    }


    private ValidPreferenceKeys _validKeys;
    private ValidPreferenceKeys ValidKeys
    {
      get 
      {
        if (_validKeys == null)
        {
          _validKeys = ValidPreferenceKeys.LoadValidPreferenceKeysFromCache();
        }
        return _validKeys;
      }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = Container.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = Container.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private string _preferencesCookieName;
    private string PreferencesCookieName
    {
      get
      {
        if (_preferencesCookieName == null)
        {
          _preferencesCookieName = "preferences" + SiteContext.PrivateLabelId;
        }
        return _preferencesCookieName;
      }
    }

    // Affiliates Legacy cookie is not supported because the cookie value
    // and what is in the preference are different formats entirely. The 
    // apps using the old cookie will have to manage preferences vs cookie logic
    // Legacy Currency we handle through the old SiteContext hooks

    // Old Currency Cookie
    private string _oldCurrencyCookieName;
    private string CurrencyCookieName
    {
      get
      {
        if (_oldCurrencyCookieName == null)
        {
          _oldCurrencyCookieName = "currency" + SiteContext.PrivateLabelId;
        }
        return _oldCurrencyCookieName;
      }
    }

    // Old Datacenter Cookie = adc{PLID}
    private string _dataCenterCookieName;
    private string DataCenterCookieName
    {
      get
      {
        if (_dataCenterCookieName == null)
        {
          _dataCenterCookieName = "adc" + SiteContext.PrivateLabelId;
        }
        return _dataCenterCookieName;
      }
    }

    // Old Flag Cookie = flag{PLID}
    private string _flagCookieName;
    private string FlagCookieName
    {
      get
      {
        if (_flagCookieName == null)
        {
          _flagCookieName = "flag" + SiteContext.PrivateLabelId;
        }
        return _flagCookieName;
      }
    }



    #endregion

    /// All save methods use the member variable _preferences if it is not null
    /// This is to prevent tornadoes
    #region Save Methods

    private void SavePreferencesToCookie()
    {
      // To avoid tornadoes this will only work once preferences have been loaded
      // Do NOT change this to use the Preferences property
      if ((_preferences != null) &&
        (HttpContext.Current != null) &&
        (HttpContext.Current.Response != null) &&
        (HttpContext.Current.Response.Cookies != null))
      {
        // Add encrypted shopper id to cookie
        string encryptedShopperId = string.Empty;
        if (!string.IsNullOrEmpty(ShopperContext.ShopperId))
        {
          encryptedShopperId = CookieHelper.EncryptCookieValue(ShopperContext.ShopperId);
        }

        HttpCookie cookie = SiteContext.NewCrossDomainMemCookie(PreferencesCookieName);
        if (cookie != null)
        {
          cookie[_SHOPPERIDPREFKEY] = encryptedShopperId;

          foreach (string key in _preferences.Keys)
          {
            if (ValidKeys.IsPreferenceKeyValid(key))
            {
              cookie[key] = _preferences[key];
            }
          }

          HttpContext.Current.Response.Cookies.Set(cookie);
        }
      }
    }

    private void SavePreferenceToLegacyCookie(string key)
    {
      if ((UseLegacyCookies) && (_preferences != null) && (_preferences.ContainsKey(key)))
      {
        string valueToSave = _preferences[key];

        switch (key)
        {
          case _CURRENCYPREFERENCEKEY:
            if ((HttpContext.Current != null) && (HttpContext.Current.Response != null) && (HttpContext.Current.Response.Cookies != null))
            {
              HttpCookie cookie = SiteContext.NewCrossDomainCookie(CurrencyCookieName, DateTime.Now.AddYears(1));
              if (cookie != null)
              {
                cookie[_LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY] = valueToSave;
                HttpContext.Current.Response.Cookies.Set(cookie);
              }
            }
            break;
          case _FLAGPREFERENCEKEY:
            {
              if ((HttpContext.Current != null) && (HttpContext.Current.Response != null) && (HttpContext.Current.Response.Cookies != null))
              {
                HttpCookie cookie = SiteContext.NewCrossDomainCookie(FlagCookieName, DateTime.Now.AddYears(1));
                if (cookie != null)
                {
                  cookie.Value = "cflag=" + valueToSave;
                  HttpContext.Current.Response.Cookies.Set(cookie);
                }
              }
            }
            break;
          case _DATACENTERPREFERENCKEY:
            {
              if ((HttpContext.Current != null) && (HttpContext.Current.Response != null) && (HttpContext.Current.Response.Cookies != null))
              {
                HttpCookie cookie = SiteContext.NewCrossDomainMemCookie(DataCenterCookieName);
                if (cookie != null)
                {
                  cookie.Value = valueToSave;
                  HttpContext.Current.Response.Cookies.Set(cookie);
                }
              }
            }
            break;
        }
      }

    }

    private void SavePreferenceToLegacyCookies(List<string> keys)
    {
      if (UseLegacyCookies)
      {
        keys.ForEach(key => SavePreferenceToLegacyCookie(key));
      }
    }

    /// To avoid tornadoes, this method does NOT use Preferences, but uses _preferences only if not null
    private void SavePreferencesToDatabase(List<string> keys)
    {
      if ((keys != null) && (_preferences != null) && (keys.Count > 0))
      {
        if ((!string.IsNullOrEmpty(ShopperContext.ShopperId)) && (IsDatabaseEnabled))
        {
          ShopperPrefUpdateRequestData request = new ShopperPrefUpdateRequestData(
            ShopperContext.ShopperId, GetSourceUrlFromContext(), string.Empty, SiteContext.Pathway, SiteContext.PageCount);
          bool hasDataToSave = false;
          foreach (string key in keys)
          {
            if (ValidKeys.IsPreferenceKeyValid(key))
            {
              request.AddPreference(key, _preferences[key]);
              hasDataToSave = true;
            }
          }

          if (hasDataToSave)
          {
            try
            {
              ShopperPrefUpdateResponseData response =
                (ShopperPrefUpdateResponseData)Engine.Engine.ProcessRequest(request, ShopperPreferencesEngineRequests.PreferencesUpdate);
            }
            catch (Exception ex)
            {
              IsDatabaseEnabled = false;
              AtlantisException aex = new AtlantisException(request, "ShopperPreferenceProvider.SavePreferencesToDatabase", ex.Message, ex.StackTrace, ex);
              Engine.Engine.LogAtlantisException(aex);
            }
          }

        }
      }
    }

    private void SavePreferenceToDatabase(string key)
    {
      SavePreferencesToDatabase(new List<string>(new[] { key }));
    }

    /// <summary>
    /// This function is to be used in cases where a new shopper is created
    /// and their session preferences need to be saved into the shopper preferences.
    /// </summary>
    public void SaveAllPreferencesToDatabase()
    {
      if (_preferences != null)
      {
        List<string> keys = new List<string>();
        foreach (string key in _preferences.Keys)
        {
          if (ValidKeys.IsPreferenceKeyValid(key))
          {
            keys.Add(key);
          }
        }

        if (keys.Count > 0)
        {
          SavePreferencesToDatabase(keys);
        }
      }
    }

    #endregion

    /// Preferences property is scoped so Save methods cannot reference
    #region Gets and Updates

    private Dictionary<string, string> _preferences;
    private Dictionary<string, string> PreferencesToGetOrUpdate
    {
      get
      {
        if (_preferences == null)
        {
          InitializePreferences();
        }
        return _preferences;
      }
    }

    /// <summary>
    /// Preferences loading works as follows.
    /// 1. If preferences cookie exists load cookie values
    /// 2. If current shopper does not match existing shopper, reload preferences from database and save cookie
    /// 3. If no preferences found, load from legacy cookies, save into preferences cookie and database
    /// 4. If still no preferences, use default
    /// 
    /// </summary>
    /// <returns></returns>
    private void InitializePreferences()
    {
      Dictionary<string, string> preferencesFoundInCookie;
      _preferences = LoadPreferencesFromCookie(out preferencesFoundInCookie);

      if (_preferences == null)
      {
        // Atempt to load from Database
        _preferences = LoadPreferencesFromDatabase();

        if ((_preferences == null) && (preferencesFoundInCookie != null) && (!string.IsNullOrEmpty(ShopperContext.ShopperId)))
        {
          _preferences = preferencesFoundInCookie;
          _preferences[_SHOPPERIDPREFKEY] = ShopperContext.ShopperId;
        }

        if ((_preferences == null) && UseLegacyCookies)
        {
          _preferences = LoadPreferencesFromLegacyCookies();
        }

        if (_preferences == null)
        {
          _preferences = new Dictionary<string, string>();
        }

        SavePreferencesToCookie();
      }

      SavePreferenceToLegacyCookies(new List<string>(_preferences.Keys));
    }

    /// <summary>
    /// Loads the preferences from the new preferences cookie
    /// </summary>
    /// <param name="preferencesFoundInCookie">output any cookie values found</param>
    /// <returns>cookie preferences IFF shopperId matches shopperId in cookie</returns>
    private Dictionary<string, string> LoadPreferencesFromCookie(out Dictionary<string, string> preferencesFoundInCookie)
    {
      preferencesFoundInCookie = null;
      Dictionary<string, string> result = null;

      if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[PreferencesCookieName];
        if ((cookie != null) && (cookie.HasKeys))
        {
          NameValueCollection cookieData = new NameValueCollection(cookie.Values);
          string encrpytedShopperId = cookieData[_SHOPPERIDPREFKEY];
          string cookieShopperId = string.Empty;
          if (!string.IsNullOrEmpty(encrpytedShopperId))
          {
            cookieShopperId = CookieHelper.DecryptCookieValue(encrpytedShopperId);
          }

          Dictionary<string, string> cookiePreferences = new Dictionary<string, string>();
          foreach (string key in cookieData.AllKeys)
          {
            if (ValidKeys.IsPreferenceKeyValid(key))
            {
              cookiePreferences[key] = cookieData[key];
            }
          }

          if (cookieShopperId == ShopperContext.ShopperId)
          {
            result = cookiePreferences;
            preferencesFoundInCookie = cookiePreferences;
          }
          else if (string.IsNullOrEmpty(cookieShopperId))
          {
            preferencesFoundInCookie = cookiePreferences;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Loads preferences from database
    /// </summary>
    /// <param name="cookiePreferences">cookie preferences (where shopperid was different or null)</param>
    /// <returns></returns>
    private Dictionary<string, string> LoadPreferencesFromDatabase()
    {
      Dictionary<string, string> result = null;
      if ((!string.IsNullOrEmpty(ShopperContext.ShopperId)) && (IsDatabaseEnabled))
      {
        ShopperPrefGetRequestData request =
          new ShopperPrefGetRequestData(ShopperContext.ShopperId, GetSourceUrlFromContext(), string.Empty, SiteContext.Pathway, SiteContext.PageCount);
        try
        {
          ShopperPrefGetResponseData response =
            (ShopperPrefGetResponseData)Engine.Engine.ProcessRequest(request, ShopperPreferencesEngineRequests.PreferencesGet);
          if (response.IsSuccess)
          {
            if (response.Preferences != null)
            {
              result = response.Preferences;
            }
          }

        }
        catch (Exception ex)
        {
          IsDatabaseEnabled = false;
          AtlantisException aex = new AtlantisException(request, "ShopperPreferenceProvider.LoadPreferencesFromDatabase", ex.Message, ex.StackTrace, ex);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }

    private string GetSourceUrlFromContext()
    {
      string result = string.Empty;
      if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        result = HttpContext.Current.Request.RawUrl;
      }

      return result;
    }

    #endregion

    #region Read Legacy Cookies

    private const string _CURRENCYPREFERENCEKEY = "gdshop_currencyType";
    private const string _FLAGPREFERENCEKEY = "countryFlag";
    private const string _DATACENTERPREFERENCKEY = "dataCenterCode";

    private Dictionary<string, string> LoadPreferencesFromLegacyCookies()
    {
      Dictionary<string, string> result = new Dictionary<string, string>();

      if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Cookies != null))
      {
        {
          // Currency
          HttpCookie currencyCookie = HttpContext.Current.Request.Cookies[CurrencyCookieName];
          if (currencyCookie != null)
          {
            string currencyValue = currencyCookie[_LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY];
            if (!string.IsNullOrEmpty(currencyValue))
            {
              result[_CURRENCYPREFERENCEKEY] = currencyValue;
            }
          }
        }

        {
          // Flag
          HttpCookie flagCookie = HttpContext.Current.Request.Cookies[FlagCookieName];
          if (flagCookie != null)
          {
            string flagValue = flagCookie.Values["cflag"];
            if (!string.IsNullOrEmpty(flagValue))
            {
              result[_FLAGPREFERENCEKEY] = flagValue;
            }
          }
        }
        
        {
          // Datacenter
          HttpCookie dataCenterCookie = HttpContext.Current.Request.Cookies[DataCenterCookieName];
          if ((dataCenterCookie != null) && (!string.IsNullOrEmpty(dataCenterCookie.Value)))
          {
            result[_DATACENTERPREFERENCKEY] = dataCenterCookie.Value;
          }
        }
      }

      if (result.Count == 0)
      {
        result = null;
      }

      return result;
    }

    #endregion

    #region IShopperPreferencesProvider Members

    private bool UpdatePreferenceInt(string key, string value)
    {
      bool result = false;
      if (!string.IsNullOrEmpty(key))
      {
        string setValue = value;
        if (setValue == null)
        {
          setValue = string.Empty;
        }

        string currentValue;
        if (!PreferencesToGetOrUpdate.TryGetValue(key, out currentValue))
        {
          currentValue = null;
        }

        if (setValue != currentValue)
        {
          PreferencesToGetOrUpdate[key] = value;
          result = true;
        }
      }
      return result;
    }

    public void UpdatePreference(string key, string value)
    {
      bool changed = UpdatePreferenceInt(key, value);
      if (changed)
      {
        SavePreferenceToDatabase(key);
        SavePreferencesToCookie();
        SavePreferenceToLegacyCookie(key);
      }
    }

    public void UpdatePreferences(IDictionary<string, string> values)
    {
      if (values != null)
      {
        List<string> updatedKeys = new List<string>(values.Count);
        foreach (string key in values.Keys)
        {
          if (UpdatePreferenceInt(key, values[key]))
          {
            updatedKeys.Add(key);
          }
        }
        SavePreferencesToDatabase(updatedKeys);
        SavePreferencesToCookie();
        SavePreferenceToLegacyCookies(updatedKeys);
      }
    }

    public string GetPreference(string key, string defaultValueIfNotFound)
    {
      string result;
      if (!PreferencesToGetOrUpdate.TryGetValue(key, out result))
      {
        result = defaultValueIfNotFound;
      }
      return result;
    }

    public bool HasPreference(string key)
    {
      return PreferencesToGetOrUpdate.ContainsKey(key);
    }

    #endregion
  }
}
