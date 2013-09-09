using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Shopper
{
  public class ShopperContextProvider : ProviderBase, IShopperContext
  {
    private const string SESSIONSECURESHOPPERKEY = "SecShopperId";
    private const string COOKIE_MEMAUTH_MID = "MemAuthId";
    private const string COOKIE_SHOPPER_MID = "ShopperId";

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

    private string LoggedInShopperId
    {
      get
      {
        string result = SafeSession.GetSessionItem(SESSIONSECURESHOPPERKEY) as string;
        return result ?? string.Empty;
      }
      set
      {
        SafeSession.SetSessionItem(SESSIONSECURESHOPPERKEY, value);
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

    protected void DeleteShopperIdMemAuthCookie()
    {
      HttpCookie memShopperCookie = HttpContext.Current.Request.Cookies[ShopperMemAuthCookieName];
      if (memShopperCookie != null)
      {
        memShopperCookie = SiteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
        memShopperCookie.Expires = DateTime.Now.AddDays(-1);
        memShopperCookie.Value = string.Empty;
        HttpContext.Current.Response.Cookies.Set(memShopperCookie);
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
        HttpContext.Current.Response.Cookies.Set(shopperCookie);
      }
    }

    protected string GetShopperIdFromMemAuthCookie()
    {
      string result = null;

      HttpCookie memShopperCookie = HttpContext.Current.Request.Cookies[ShopperMemAuthCookieName];
      if (memShopperCookie != null)
      {
        result = memShopperCookie.Value;
      }

      return result;
    }

    protected string GetShopperIdFromCrossDomainCookie()
    {
      string result = null;

      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[CrossDomainShopperCookieName];
      if (shopperCookie != null)
      {
        result = shopperCookie.Value;
      }

      return result;
    }

    private void SaveShopperIdToCookie(string shopperId)
    {
      HttpCookie shopperCookie = HttpContext.Current.Request.Cookies[CrossDomainShopperCookieName];

      if (shopperCookie == null || shopperCookie.Value != shopperId)
      {
        shopperCookie = SiteContext.NewCrossDomainCookie(CrossDomainShopperCookieName, DateTime.UtcNow.AddYears(10));
        string encryptedShopperId = string.Empty;
        if (!string.IsNullOrEmpty(shopperId))
        {
          encryptedShopperId = PublicCookieData.CreateEncrypted(shopperId);
        }
        shopperCookie.Value = encryptedShopperId;
        HttpContext.Current.Response.Cookies.Set(shopperCookie);
      }
    }

    private void SaveShopperAuthDataToMemCookie(string shopperId)
    {
      string memAuthData = MemAuthCookieData.CreateEncrypted(shopperId, DateTime.Now, SiteContext.PrivateLabelId);
      HttpCookie shopperCookie = SiteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
      shopperCookie.Value = memAuthData;
      HttpContext.Current.Response.Cookies.Set(shopperCookie);
    }

    private void DetermineShopperId()
    {
      _shopperId = string.Empty;
      _status = ShopperStatusType.Public;

      try
      {
        if (IsManager)
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
          }
          else // we have a shopper cookie
          {
            PublicCookieData crossDomainCookieData = new PublicCookieData(encryptedCrossDomainShopperId);
            if (string.IsNullOrEmpty(crossDomainCookieData.ShopperId))
            {
              if (!string.IsNullOrEmpty(encryptedMemAuthShopperId))
              {
                DeleteShopperIdMemAuthCookie();
              }
              LoggedInShopperId = string.Empty;
            }
            else
            {
              _shopperId = crossDomainCookieData.ShopperId;
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
        }
      }
      catch (Exception ex)
      {
        string message = "Error determining ShopperId" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("ShopperContextProvider.DetermineShopperId", 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

    }

    public bool SetLoggedInShopper(string shopperId)
    {
      bool result = false;

      if (!IsManager)
      {
        // Setting the logged in shopper can only occur after
        // a valid IDP login event.  In that case, the shopperId
        // passed into this function should match the partially
        // trusted shopper id
        if (!string.IsNullOrEmpty(shopperId) && (shopperId == ShopperId) && (ShopperStatus == ShopperStatusType.PartiallyTrusted))
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

      if (!IsManager)
      {
        SaveShopperIdToCookie(shopperId);
        SaveShopperAuthDataToMemCookie(shopperId);
        LoggedInShopperId = shopperId;
        _shopperId = shopperId;
        _status = ShopperStatusType.Authenticated;
        result = true;
      }

      return result;
    }

    public void SetNewShopper(string shopperId)
    {
      if (!IsManager)
      {
        LoggedInShopperId = string.Empty;
        DeleteShopperIdMemAuthCookie();
        SaveShopperIdToCookie(shopperId);
        _shopperId = shopperId;
        _status = ShopperStatusType.Public;
      }
    }

    public void ClearShopper()
    {
      if (!IsManager)
      {
        LoggedInShopperId = string.Empty;
        DeleteShopperIdMemAuthCookie();
        DeleteShopperIdCrossDomainCookie();
        _shopperId = string.Empty;
        _status = ShopperStatusType.Public;
      }
    }

    private bool IsManager
    {
      get
      {
        bool result = false;
        if (SiteContext.Manager != null)
        {
          result = SiteContext.Manager.IsManager;
        }
        return result;
      }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return _status; }
    }

    #region ShopperPriceType

    private Lazy<ShopperSpecificSessionDataItem<int>> _shopperPriceTypeSessionData = 
      new Lazy<ShopperSpecificSessionDataItem<int>>(() => { return new ShopperSpecificSessionDataItem<int>("ShopperContextProvider.ShopperPriceType");});

    public int ShopperPriceType
    {
      get
      {
        return GetShopperPriceType();
      }
    }

    private int GetShopperPriceType()
    {
      if (string.IsNullOrEmpty(ShopperId))
      {
        return 0;
      }

      int shopperPriceType;
      if (_shopperPriceTypeSessionData.Value.TryGetData(ShopperId, out shopperPriceType))
      {
        return shopperPriceType;
      }

      try
      {
        var request = new ShopperPriceTypeRequestData(ShopperId, SiteContext.PrivateLabelId);
        var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.ShopperPriceType);
        _shopperPriceTypeSessionData.Value.SetData(ShopperId, response.ActivePriceType);
        return response.ActivePriceType;
      }
      catch(Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = "shopperid=" + ShopperId + ":" + SiteContext.PrivateLabelId.ToString();
        var aex = new AtlantisException("ShopperContextProvider.GetShopperPriceType", 0, message, data);      
        Engine.Engine.LogAtlantisException(aex);
      }

      _shopperPriceTypeSessionData.Value.SetData(ShopperId, 0);
      return 0;
    }

    #endregion
  }
}
