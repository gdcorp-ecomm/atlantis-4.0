using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Sso.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

namespace Atlantis.Framework.Providers.Sso
{
  /// <summary>
  /// Summary description for TokenSsoProvider
  /// </summary>
  public abstract class TokenSsoProviderBase : ProviderBase, ITokenSsoProvider
  {
    private const int _DEFAULT_TOKEN_TIMEOUT_MINS = 240;
    private Lazy<Token> _token;
    private Lazy<IShopperContext> _shopperContext;
    private Lazy<ISiteContext> _siteContext;
    private bool? _isTokenValid;

    /// <summary>
    /// The amount of time in minutes the token is valid after the Issued at Time.  If the issue at time + the timeout is less than Now than 
    /// then we should force the user to log in again.  The token will also be viewed as invalid if IAT is not valid.
    /// </summary>
    public virtual int TokenTimeoutMins
    {
      get
      {
        return _DEFAULT_TOKEN_TIMEOUT_MINS;
      }
    }

    /// <summary>
    /// Gets the logged in shopper according to your application specific logic. For example, IDP uses a session key to get the logged in shopper.
    /// </summary>
    public abstract string CurrentLoggedInShopperId { get; }

    public bool TokenHasData
    {
      get { return !string.IsNullOrEmpty(Token.data); }
    }

    public Token Token
    {
      get { return _token.Value; }
    }

    /// <summary>
    /// Looks to see if the currently logged in shopper matches the shopper id from the token.
    /// </summary>
    public bool CurrentShopperIsTokenShopper
    {
      get
      {
        return CurrentLoggedInShopperId.Equals(Token.Payload.shopperId, StringComparison.OrdinalIgnoreCase);
      }
    }

    protected TokenSsoProviderBase(IProviderContainer container)
      : base(container)
    {
      _token = new Lazy<Token>(() => GetTokenAndSetValues());
      _shopperContext = new Lazy<IShopperContext>(() => container.Resolve<IShopperContext>());
      _siteContext = new Lazy<ISiteContext>(() => container.Resolve<ISiteContext>());
    }

    public bool IsTokenValid()
    {
      if (!_isTokenValid.HasValue)
      {
        _isTokenValid = false;

        try
        {
          bool issuedAtTimeIsValid = Token.IssuedAt.AddMinutes(TokenTimeoutMins) > DateTime.Now;
          bool expiredTimeIsValid = Token.ExpiresAt > DateTime.Now;

          _isTokenValid = issuedAtTimeIsValid && expiredTimeIsValid && Token.IsSignatureValid;
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("TokenSsoProvider::IsTokenValid", 1, "Error verifying token data", ex.Message);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return _isTokenValid.Value;
    }

    public virtual void SetLoggedInShopper()
    {
      if (IsTokenValid())
      {
        _shopperContext.Value.SetLoggedInShopperWithCookieOverride(Token.Payload.shopperId);
      }
    }

    private Token GetTokenAndSetValues()
    {
      Token returnToken = new Token();

      var tokenCookie = HttpContext.Current.Request.Cookies[GetTokenCookieName()];

      if (tokenCookie != null)
      {
        returnToken.data = tokenCookie.Value;
        returnToken.PrivateLabelId = _siteContext.Value.PrivateLabelId;
      }

      return returnToken;
    }

    private string GetTokenCookieName()
    {
      string cookieName = string.Empty;

      switch (_siteContext.Value.ServerLocation)
      {
        case ServerLocationType.Dev:
          cookieName = "devauth_idp";
          break;
        case ServerLocationType.Test:
          cookieName = "testauth_idp";
          break;
        case ServerLocationType.Ote:
          cookieName = "oteauth_idp";
          break;
        default:
          cookieName = "auth_idp";
          break;
      }

      cookieName += _siteContext.Value.PrivateLabelId;
      return cookieName;
    }
  }

}
