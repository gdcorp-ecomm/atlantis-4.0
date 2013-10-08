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
        try
        {
          TokenType tokenType;
          if (Enum.TryParse(token.RawTokenData, true, out tokenType))
          {
            var data = GetProviderInfo();

            string result = String.Empty;
            switch (tokenType)
            {
              case TokenType.SpKey:
                result = data.SpKey;
                break;
              case TokenType.SpGroupName:
                result = data.SpGroupName;
                break;
              case TokenType.LogInUrl:
                result = data.LogInUrl;
                break;
              case TokenType.LogOutUrl:
                result = data.LogOutUrl;
                break;
            }

            token.TokenResult = result;
            returnValue = true;
          }
          else
          {
            throw new ApplicationException(String.Format("Unable to retrieve Identity Provider Information for {0}.", cast.RawTokenData));
          }
        }
        catch (Exception ex)
        {
          returnValue = false;
          token.TokenResult = string.Empty;
          token.TokenError = ex.Message;
          LogDebugMessage(ex.Message, ex.Source);
        }
      }

      return returnValue;
    }

    private IdentityProviderInfo GetProviderInfo()
    {
      IdentityProviderInfo returnValue = new IdentityProviderInfo();

      try
      {
        var ssoProvider = ProviderContainer.Resolve<ISsoProvider>();
        returnValue.SpKey = ssoProvider.SpKey;
        returnValue.SpGroupName = ssoProvider.ServiceProviderGroupName;
        returnValue.LogInUrl = ssoProvider.GetUrl(SsoUrlType.Login);
        returnValue.LogOutUrl = ssoProvider.GetUrl(SsoUrlType.Logout);
      }
      catch (Exception ex)
      {
        LogDebugMessage(ex.Message, ex.Source);
      }

      return returnValue;
    }

    private void LogDebugMessage(string message, string errorSource)
    {
      if (this.ProviderContainer.CanResolve<IDebugContext>())
      {
        this.ProviderContainer.Resolve<IDebugContext>().LogDebugTrackingData(errorSource, message);
      }
    }

  }
}
