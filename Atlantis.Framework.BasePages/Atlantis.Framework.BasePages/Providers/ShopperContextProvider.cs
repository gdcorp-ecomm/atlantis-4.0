using Atlantis.Framework.BasePages.Cookies;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperPriceType.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.BasePages.Providers
{
  public class ShopperContextProvider : ProviderBase, IShopperContext
  {
    public static TimeSpan ShopperPriceTypeCacheTime { get; set; }

    static ShopperContextProvider()
    {
      ShopperPriceTypeCacheTime = TimeSpan.FromMinutes(10);
    }

    private const string SESSION_SECURE_SHOPPER_ID_PREFIX = "SecShopperId";
    private const string COOKIE_MEMAUTH_MID = "MemAuthId";
    private const string COOKIE_SHOPPER_MID = "ShopperId";
    private const string COOKIE_SHOPPER_OLD_PREFIX = "ShopperId-";

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

    private string _shopperId;
    private ShopperStatusType _status;

    public ShopperContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      DetermineShopperId();
    }

    private string _secShopperIdKey;
    private string SecShopperIdSessonKey
    {
      get
      {
        if (_secShopperIdKey == null)
        {
          _secShopperIdKey = SESSION_SECURE_SHOPPER_ID_PREFIX + SiteContext.PrivateLabelId;
        }
        return _secShopperIdKey;
      }
    }


    private string LoggedInShopperId
    {
      get
      {
        string result = null;

        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
          result = HttpContext.Current.Session[SecShopperIdSessonKey] as string;
        }

        return result ?? string.Empty;
      }
      set
      {
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
          HttpContext.Current.Session[SecShopperIdSessonKey] = value;
        }
      }
    }

    private string CookieLocationPrefix
    {
      get
      {
        string result = string.Empty;
        if (SiteContext.ServerLocation == ServerLocationType.Test)
        {
          result = "test";
        }
        else if (SiteContext.ServerLocation == ServerLocationType.Dev)
        {
          result = "dev";
        }
        else if (SiteContext.ServerLocation == ServerLocationType.Ote)
        {
          result = "ote";
        }
        return result;
      }
    }

    protected string ShopperMemAuthCookieName
    {
      get { return CookieLocationPrefix + COOKIE_MEMAUTH_MID + SiteContext.PrivateLabelId; }
    }

    protected string CrossDomainShopperCookieName
    {
      get { return CookieLocationPrefix + COOKIE_SHOPPER_MID + SiteContext.PrivateLabelId; }
    }

    protected string OldShopperCookieName
    {
      get { return COOKIE_SHOPPER_OLD_PREFIX + SiteContext.PrivateLabelId; }
    }

    protected void DeleteShopperIdMemAuthCookie()
    {
      HttpCookie memShopperCookie = HttpContext.Current.Request.Cookies[ShopperMemAuthCookieName];
      if (memShopperCookie != null)
      {
        memShopperCookie = SiteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
        memShopperCookie.Expires = DateTime.Now.AddDays(-1);
        memShopperCookie.Value = string.Empty;
        HttpContext.Current.Response.Cookies.Add(memShopperCookie);
      }
    }

    protected void DeleteShopperIdCrossDomainCookie()
    {
      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[CrossDomainShopperCookieName];
      if (shopperCookie != null)
      {
        shopperCookie = SiteContext.NewCrossDomainMemCookie(CrossDomainShopperCookieName);
        shopperCookie.Expires = DateTime.Now.AddDays(-1);
        shopperCookie.Value = string.Empty;
        HttpContext.Current.Response.Cookies.Add(shopperCookie);
      }
    }

    protected string GetShopperIdFromMemAuthCookie()
    {
      string sResult = null;

      HttpCookie memShopperCookie = HttpContext.Current.Request.Cookies[ShopperMemAuthCookieName];
      if (memShopperCookie != null)
      {
        sResult = memShopperCookie.Value;
      }

      return sResult;
    }

    private void DeleteShopperOldCookie()
    {
      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[OldShopperCookieName];
      if (shopperCookie != null)
      {
        shopperCookie.Expires = DateTime.Now.AddDays(-7);
        shopperCookie.Value = string.Empty;
        HttpContext.Current.Response.Cookies.Add(shopperCookie);
      }
    }

    protected string GetShopperIdFromOldCookie()
    {
      string sResult = null;
      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[OldShopperCookieName];
      if (shopperCookie != null)
      {
        sResult = shopperCookie.Value;
      }
      return sResult;
    }

    protected string GetShopperIdFromCrossDomainCookie()
    {
      string sResult = null;

      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[CrossDomainShopperCookieName];
      if (shopperCookie != null)
        sResult = shopperCookie.Value;

      return sResult;
    }

    private void SaveShopperIdToCookie(string shopperId)
    {
      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[CrossDomainShopperCookieName];

      if (shopperCookie == null
        || shopperCookie.Value != shopperId)
      {
        shopperCookie = SiteContext.NewCrossDomainCookie(CrossDomainShopperCookieName, DateTime.UtcNow.AddYears(10));
        string encryptedShopperId = string.Empty;
        if (!string.IsNullOrEmpty(shopperId))
        {
          encryptedShopperId = CookieHelper.EncryptCookieValue(shopperId);
        }
        shopperCookie.Value = encryptedShopperId;
        HttpContext.Current.Response.Cookies.Add(shopperCookie);
      }
    }

    private void SaveShopperAuthDataToMemCookie(string shopperId)
    {
      string delimitedData = shopperId + "|" + DateTime.Now + "|" + SiteContext.PrivateLabelId;
      string memAuthData = CookieHelper.EncryptCookieValue(delimitedData);
      HttpCookie shopperCookie = SiteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
      shopperCookie.Value = memAuthData;
      HttpContext.Current.Response.Cookies.Add(shopperCookie);
    }

    private void DetermineShopperId()
    {
      _shopperId = string.Empty;
      _status = ShopperStatusType.Public;
      _shopperPriceType = null;

      try
      {
        if ((SiteContext.Manager != null) && (SiteContext.Manager.IsManager))
        {
          _shopperId = SiteContext.Manager.ManagerShopperId;
          _status = ShopperStatusType.Manager;
        }
        else
        {
          string encryptedCrossDomainShopperId = GetShopperIdFromCrossDomainCookie();
          string encryptedMemAuthShopperId = GetShopperIdFromMemAuthCookie();

          // Validation
          if (string.IsNullOrEmpty(encryptedCrossDomainShopperId))
          {
            if (!string.IsNullOrEmpty(encryptedMemAuthShopperId))
            {
              DeleteShopperIdMemAuthCookie();
            }
            LoggedInShopperId = string.Empty;

            // last check for old shopper id
            string encryptedOldShopperId = GetShopperIdFromOldCookie();
            if (!string.IsNullOrEmpty(encryptedOldShopperId))
            {
              string decrypted = CookieHelper.DecryptCookieValue(encryptedOldShopperId);
              if (!string.IsNullOrEmpty(decrypted))
              {
                _shopperId = decrypted;
                SaveShopperIdToCookie(_shopperId);
              }
            }
          }
          else // we have a shopper cookie
          {
            string decrypted = CookieHelper.DecryptCookieValue(encryptedCrossDomainShopperId);
            if (string.IsNullOrEmpty(decrypted))
            {
              if (!string.IsNullOrEmpty(encryptedMemAuthShopperId))
              {
                DeleteShopperIdMemAuthCookie();
              }
              LoggedInShopperId = string.Empty;
            }
            else
            {
              _shopperId = decrypted;
              if (string.IsNullOrEmpty(encryptedMemAuthShopperId))
              {
                LoggedInShopperId = string.Empty;
              }
              else
              {
                MemAuthCookieData memAuthCookieData = new MemAuthCookieData(encryptedMemAuthShopperId);
                if (memAuthCookieData.ShopperId != _shopperId)
                {
                  DeleteShopperIdMemAuthCookie();
                  LoggedInShopperId = string.Empty;
                }
                else
                {
                  _status = ShopperStatusType.PartiallyTrusted;
                  if (LoggedInShopperId != _shopperId)
                  {
                    LoggedInShopperId = string.Empty;
                  }
                  else
                  {
                    _status = ShopperStatusType.Authenticated;
                  }
                }

              }
            }
          }
          DeleteShopperOldCookie();
        }
      }
      catch (Exception ex)
      {
        string message = "Error determining ShopperId" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("SalesBasePage.DeterminShopperId",
          HttpContext.Current.Request.Url.ToString(), "0", message, message, string.Empty, string.Empty,
          HttpContext.Current.Request.UserHostAddress, string.Empty, 0);
        Engine.Engine.LogAtlantisException(aex);
      }

    }

    public bool SetLoggedInShopper(string shopperId)
    {
      bool result = false;

      if (!SiteContext.Manager.IsManager)
      {
        // Setting the logged in shopper can only occur after
        // a valid IDP login event.  In that case, the shopperId
        // passed into this function should match the partially
        // trusted shopper id
        if (!string.IsNullOrEmpty(shopperId) && (shopperId == ShopperId) && (Status == ShopperStatusType.PartiallyTrusted))
        {
          LoggedInShopperId = shopperId;
          _status = ShopperStatusType.Authenticated;
          result = true;
        }
      }

      return result;
    }

    /// <summary>
    /// Sets the cookies AND the logged in shopper.  This should ONLY be used by SSO or when you
    /// are logging in a shopper via SSO and the hosts do not match so the cookies from SSO will not
    /// be readable by your app. Ensure you get a design review if you are using this method.
    /// </summary>
    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      bool result = false;

      if (!SiteContext.Manager.IsManager)
      {
        SaveShopperIdToCookie(shopperId);
        SaveShopperAuthDataToMemCookie(shopperId);
        LoggedInShopperId = shopperId;
        _shopperId = shopperId;
        _status = ShopperStatusType.Authenticated;
        _shopperPriceType = null;
        result = true;
      }

      return result;
    }

    public void SetNewShopper(string shopperId)
    {
      if (!SiteContext.Manager.IsManager)
      {
        LoggedInShopperId = string.Empty;
        DeleteShopperIdMemAuthCookie();
        SaveShopperIdToCookie(shopperId);
        _shopperId = shopperId;
        _status = ShopperStatusType.Public;
        _shopperPriceType = null;
      }
    }

    public void ClearShopper()
    {
      if (!SiteContext.Manager.IsManager)
      {
        LoggedInShopperId = string.Empty;
        DeleteShopperIdMemAuthCookie();
        DeleteShopperIdCrossDomainCookie();
        _shopperId = string.Empty;
        _status = ShopperStatusType.Public;
        _shopperPriceType = null;
      }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType Status
    {
      get { return _status; }
    }

    private class MemAuthCookieData
    {
      public string ShopperId = string.Empty;
      public string LoginDate = string.Empty;
      public string PrivateLabelId = string.Empty;

      public MemAuthCookieData(string encryptedCookieValue)
      {
        string memAuthData = string.Empty;
        if (!string.IsNullOrEmpty(encryptedCookieValue))
        {
          memAuthData = CookieHelper.DecryptCookieValue(encryptedCookieValue);
        }
        if (!string.IsNullOrEmpty(memAuthData))
        {
          string[] parts = memAuthData.Split('|');
          if (parts.Length > 0)
          {
            ShopperId = parts[0];
          }

          if (parts.Length > 1)
          {
            LoginDate = parts[1];
          }

          if (parts.Length > 2)
          {
            PrivateLabelId = parts[2];
          }
        }
      }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return _status; }
    }

    #region ShopperPriceType

    private const string _shopperPriceTypeSessionKeyPrefix = "BasePages.ShopperContext.ShopperPriceType.";
    private int? _shopperPriceType = null;

    public int ShopperPriceType
    {
      get
      {
        if (!_shopperPriceType.HasValue)
        {
          if (HttpContext.Current.Session != null)
          {
            _shopperPriceType = GetShopperPriceTypeFromSessionCache();
          }
          else
          {
            //ShopperPriceType won't be cached since "session is null".  The application needs to implement its own caching.
            //For example, CIF is a WCF service and it doesn't have "session".  So, CIF needs to implement its own caching for this data.
            _shopperPriceType = GetShopperPriceType();
          }
        }
        return _shopperPriceType.Value;
      }
    }

    private int GetShopperPriceTypeFromSessionCache()
    {
      int result = 0; //ShopperPriceTypes.Standard;

      if (!string.IsNullOrEmpty(ShopperId))
      {
        ShopperPriceTypeRequestData request = new ShopperPriceTypeRequestData(
          ShopperId, HttpContext.Current.Request.Url.ToString(),
          string.Empty, SiteContext.Pathway, SiteContext.PageCount, SiteContext.PrivateLabelId);
        try
        {
          ShopperPriceTypeResponseData response =
            SessionCache.SessionCache.GetProcessRequest<ShopperPriceTypeResponseData>(request, BasePageEngineRequests.ShopperPriceType, ShopperPriceTypeCacheTime);
          result = response.ActivePriceType;
        }
        catch (Exception ex)
        {
          string msg = "Error getting pricetype for " + ShopperId + ":" + SiteContext.PrivateLabelId.ToString() + Environment.NewLine +
            ex.Message + Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException(request, "ShopperContextProvider.GetShopperPriceTypeFromService", msg, request.ToXML());
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }

    private int GetShopperPriceType()
    {
      int result = 0; //ShopperPriceTypes.Standard;

      if (!string.IsNullOrEmpty(ShopperId))
      {
        ShopperPriceTypeRequestData request = new ShopperPriceTypeRequestData(
          ShopperId, HttpContext.Current.Request.Url.ToString(),
          string.Empty, SiteContext.Pathway, SiteContext.PageCount, SiteContext.PrivateLabelId);
        try
        {
          ShopperPriceTypeResponseData response = (ShopperPriceTypeResponseData)
            Engine.Engine.ProcessRequest(request, BasePageEngineRequests.ShopperPriceType);
          result = response.ActivePriceType;
        }
        catch (Exception ex)
        {
          string msg = "Error getting pricetype for " + ShopperId + ":" + SiteContext.PrivateLabelId.ToString() + Environment.NewLine +
            ex.Message + Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException(request, "ShopperContextProvider.GetShopperPriceTypeFromService", msg, request.ToXML());
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }

    #endregion
  }
}
