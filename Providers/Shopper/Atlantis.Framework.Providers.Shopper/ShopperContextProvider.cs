using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SsoAuth.Interface;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Shopper
{
  public class ShopperContextProvider : ProviderBase, IShopperContext
  {
    private const string _SESSIONSECURESHOPPERKEY = "SecShopperId";
    private const string _COOKIE_MEMAUTH_MID = "MemAuthId";
    private const string _COOKIE_SHOPPER_MID = "ShopperId";

    private readonly IManagerContext _managerContext;
    private readonly ISiteContext _siteContext;
    private readonly ShopperIdAndStatus _shopperIdAndStatus; 

    public ShopperContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      _siteContext = Container.Resolve<ISiteContext>();
      Container.TryResolve(out _managerContext);
      _shopperIdAndStatus = new ShopperIdAndStatus(Container);

      DetermineShopperId();
    }

    private string LoggedInShopperId
    {
      get
      {
        var result = SafeSession.GetSessionItem(_SESSIONSECURESHOPPERKEY) as string;
        return result ?? string.Empty;
      }
      set
      {
        SafeSession.SetSessionItem(_SESSIONSECURESHOPPERKEY, value);
      }
    }

    private string CookieLocationPrefix
    {
      get
      {
        string result = string.Empty;
        if (_siteContext.ServerLocation == ServerLocationType.Test)
        {
          result = "test";
        }
        else if (_siteContext.ServerLocation == ServerLocationType.Dev)
        {
          result = "dev";
        }
        else if (_siteContext.ServerLocation == ServerLocationType.Ote)
        {
          result = "ote";
        }
        return result;
      }
    }

    protected string ShopperMemAuthCookieName
    {
      get { return CookieLocationPrefix + _COOKIE_MEMAUTH_MID + _siteContext.PrivateLabelId; }
    }

    protected string CrossDomainShopperCookieName
    {
      get { return CookieLocationPrefix + _COOKIE_SHOPPER_MID + _siteContext.PrivateLabelId; }
    }

    protected void DeleteShopperIdMemAuthCookie()
    {
      HttpCookie memShopperCookie = HttpContext.Current.Request.Cookies[ShopperMemAuthCookieName];
      if (memShopperCookie != null)
      {
        memShopperCookie = _siteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
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
        shopperCookie = _siteContext.NewCrossDomainMemCookie(CrossDomainShopperCookieName);
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
        shopperCookie = _siteContext.NewCrossDomainCookie(CrossDomainShopperCookieName, DateTime.UtcNow.AddYears(10));
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
      string memAuthData = MemAuthCookieData.CreateEncrypted(shopperId, DateTime.Now, _siteContext.PrivateLabelId);
      HttpCookie shopperCookie = _siteContext.NewCrossDomainMemCookie(ShopperMemAuthCookieName);
      shopperCookie.Value = memAuthData;
      HttpContext.Current.Response.Cookies.Set(shopperCookie);
    }

    private void DetermineShopperId()
    {
      try
      {
        if (CheckForManagerShopperId())
        {
          return;
        }

        if (CheckForAuthTokenShopperId())
        {
          return;
        }

        ReadIdpCookiesForShopperId();
      }
      catch (Exception ex)
      {
        var message = "Error determining ShopperId" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("ShopperContextProvider.DetermineShopperId", 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    /// <summary>
    /// Checks the shopperId cookie and the mem cookie against the session to establish shopper id and authentication status.
    /// This implementation will be obsolete when it IDP is fully replaced by the auth token used in CheckForAuthTokenShopperId()
    /// </summary>
    private void ReadIdpCookiesForShopperId()
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

        _shopperIdAndStatus.SetShopperAndStatus(string.Empty, ShopperStatusType.Public, "IdpCookies");
        return;
      }

      // we have a shopper cookie
      var crossDomainCookieData = new PublicCookieData(encryptedCrossDomainShopperId);
      if (string.IsNullOrEmpty(crossDomainCookieData.ShopperId))
      {
        if (!string.IsNullOrEmpty(encryptedMemAuthShopperId))
        {
          DeleteShopperIdMemAuthCookie();
        }

        LoggedInShopperId = string.Empty;
        return;
      }

      string foundShopperId = crossDomainCookieData.ShopperId;
      if (string.IsNullOrEmpty(encryptedMemAuthShopperId))
      {
        LoggedInShopperId = string.Empty;
        _shopperIdAndStatus.SetShopperAndStatus(foundShopperId, ShopperStatusType.Public, "IdpCookies");
        return;
      }

      var memAuthCookieData = new MemAuthCookieData(encryptedMemAuthShopperId);
      if (memAuthCookieData.ShopperId != foundShopperId)
      {
        DeleteShopperIdMemAuthCookie();
        LoggedInShopperId = string.Empty;
        _shopperIdAndStatus.SetShopperAndStatus(foundShopperId, ShopperStatusType.Public, "IdpCookies");
        return;
      }

      var foundStatus = ShopperStatusType.PartiallyTrusted;
      if (LoggedInShopperId != foundShopperId)
      {
        LoggedInShopperId = string.Empty;
      }
      else
      {
        foundStatus = ShopperStatusType.Authenticated;
      }

      _shopperIdAndStatus.SetShopperAndStatus(foundShopperId, foundStatus, "IdpCookies");
    }

    private bool CheckForAuthTokenShopperId()
    {
      if (Container.CanResolve<IAuthTokenProvider>())
      {
        var authTokenProvider = Container.Resolve<IAuthTokenProvider>();
        var authToken = authTokenProvider.AuthToken;
        if (authToken != null && authToken.Validate())
        {
          SaveShopperIdToCookie(authToken.Payload.ShopperId);
          LoggedInShopperId = authToken.Payload.ShopperId;

          _shopperIdAndStatus.SetShopperAndStatus(authToken.Payload.ShopperId, ShopperStatusType.Authenticated, "AuthToken");
          return true;
        }
      }
      return false;
    }

    private bool CheckForManagerShopperId()
    {
      if (IsManager)
      {
        _shopperIdAndStatus.SetShopperAndStatus(_managerContext.ManagerShopperId, ShopperStatusType.Manager, "ManagerContext");
        return true;
      }
      return false;
    }

    [Obsolete("The IDP system is being replaced by the SSO auth token. The redirect loop that results in this code is not required.")]
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
          _shopperIdAndStatus.SetShopperAndStatus(shopperId, ShopperStatusType.Authenticated, "SetLoggedInShopper");
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
    [Obsolete("The IDP system is being replaced by the SSO auth token. Domains will need to match for cross cookie usage. If a gap still exists, implement it in the SsoAuth Provider.")]
    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      bool result = false;

      if (!IsManager)
      {
        if (Container.CanResolve<IAuthTokenProvider>())
        {
          var authTokenProvider = Container.Resolve<IAuthTokenProvider>();
          var authToken = authTokenProvider.AuthToken;
          if (authToken != null && authToken.Payload != null && shopperId != authToken.Payload.ShopperId && Container.CanResolve<IAuthenticationProvider>())
          {
            var authenticationProvider = Container.Resolve<IAuthenticationProvider>();
            authenticationProvider.Deauthenticate();
          }
        }

        SaveShopperIdToCookie(shopperId);
        SaveShopperAuthDataToMemCookie(shopperId);
        LoggedInShopperId = shopperId;

        _shopperIdAndStatus.SetShopperAndStatus(shopperId, ShopperStatusType.Authenticated, "SetLoggedInShopperWithCookieOverride");
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
        if (Container.CanResolve<IAuthenticationProvider>())
        {
          var authenticationProvider = Container.Resolve<IAuthenticationProvider>();
          authenticationProvider.Deauthenticate();
        }

        _shopperIdAndStatus.SetShopperAndStatus(shopperId, ShopperStatusType.Public, "SetNewShopper");
      }
    }

    public void ClearShopper()
    {
      if (!IsManager)
      {
        LoggedInShopperId = string.Empty;
        DeleteShopperIdMemAuthCookie();
        DeleteShopperIdCrossDomainCookie();
        if (Container.CanResolve<IAuthenticationProvider>())
        {
          var authenticationProvider = Container.Resolve<IAuthenticationProvider>();
          authenticationProvider.Deauthenticate();
        }

        _shopperIdAndStatus.SetShopperAndStatus(string.Empty, ShopperStatusType.Public, "ClearShopper");
      }
    }

    private bool IsManager
    {
      get
      {
        bool result = false;
        if (_managerContext != null)
        {
          result = _managerContext.IsManager;
        }
        return result;
      }
    }

    public string ShopperId
    {
      get { return _shopperIdAndStatus.ShopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return _shopperIdAndStatus.Status; }
    }

    #region ShopperPriceType

    private readonly Lazy<ShopperSpecificSessionDataItem<int>> _shopperPriceTypeSessionData = 
      new Lazy<ShopperSpecificSessionDataItem<int>>(() => new ShopperSpecificSessionDataItem<int>("ShopperContextProvider.ShopperPriceType"));

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
        var request = new ShopperPriceTypeRequestData(ShopperId, _siteContext.PrivateLabelId);
        var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.ShopperPriceType);
        _shopperPriceTypeSessionData.Value.SetData(ShopperId, response.ActivePriceType);
        return response.ActivePriceType;
      }
      catch(Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = "shopperid=" + ShopperId + ":" + _siteContext.PrivateLabelId;
        var aex = new AtlantisException("ShopperContextProvider.GetShopperPriceType", 0, message, data);      
        Engine.Engine.LogAtlantisException(aex);
      }

      _shopperPriceTypeSessionData.Value.SetData(ShopperId, 0);
      return 0;
    }

    #endregion
  }
}
