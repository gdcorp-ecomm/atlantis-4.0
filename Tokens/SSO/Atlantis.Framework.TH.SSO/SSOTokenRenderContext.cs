using System;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System.Xml.Linq;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.Providers.Interface.Links;
using System.Linq;
using Atlantis.Framework.Providers.Sso.Interface;

namespace Atlantis.Framework.TH.SSO
{
  public enum TokenType
  {
    SpKey,
    SpGroupName,
    LogInUrl,
    LogOutUrl
  }

  public class SSOTokenRenderContext
  {

    private struct IdentityProviderInfo
    {
      public string SpKey { get; set; }
      public string SpGroupName { get; set; }
      public string LogInUrl { get; set; }
      public string LogOutUrl { get; set; }
    }

    public SSOTokenRenderContext(IProviderContainer providerContainer)
    {
      if (ReferenceEquals(null, providerContainer))
        throw new ArgumentNullException("providerContainer", "providerContainer is null.");

      ProviderContainer = providerContainer;
    }

    public IProviderContainer ProviderContainer { get; private set; }

    public bool RenderToken(IToken token)
    {
      bool returnValue = false;

      SimpleToken cast = token as SimpleToken;

      if (!ReferenceEquals(null, cast))
      {
        TokenType tokenType;
        ISsoProvider ssoProvider = null;
        if (Enum.TryParse(token.RawTokenData, true, out tokenType) && ProviderContainer.TryResolve<ISsoProvider>(out ssoProvider))
        {
          string result = String.Empty;
          switch (tokenType)
          {
            case TokenType.SpKey:
              result = ssoProvider.SpKey;
              break;
            case TokenType.SpGroupName:
              result = ssoProvider.ServiceProviderGroupName;
              break;
            case TokenType.LogInUrl:
              result = ssoProvider.GetUrl(SsoUrlType.Login);
              break;
            case TokenType.LogOutUrl:
              result = ssoProvider.GetUrl(SsoUrlType.Logout);
              break;
          }
          token.TokenResult = result;
          returnValue = true;
        }
        else
        {
          string errorMsg = "Unable to retrieve Identity Provider Information.";
          token.TokenResult = string.Empty;
          token.TokenError = errorMsg;
          Engine.Engine.LogAtlantisException(new AtlantisException("RenderToken", 0, errorMsg, cast.RawTokenData));
        }
      }

      return returnValue;
    }

  }
}
