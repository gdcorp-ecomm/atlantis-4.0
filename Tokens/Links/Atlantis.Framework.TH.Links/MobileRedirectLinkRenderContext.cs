using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Links
{
  internal class MobileRedirectLinkRenderContext
  {
    private Lazy<IMobileRedirectProvider> _mobileRedirectProvider;
    
    internal MobileRedirectLinkRenderContext(IProviderContainer providerContainer)
    {
      _mobileRedirectProvider = new Lazy<IMobileRedirectProvider>(providerContainer.Resolve<IMobileRedirectProvider>); 
    }

    internal bool RenderToken(IToken token)
    {
      bool result = false;

      MobileRedirectLinkToken mobileRedirectLinkToken = token as MobileRedirectLinkToken;
      
      if (mobileRedirectLinkToken != null)
      {
        if (!string.IsNullOrEmpty(mobileRedirectLinkToken.RedirectKey))
        {
          string redirectUrl = _mobileRedirectProvider.Value.GetRedirectUrl(mobileRedirectLinkToken.RedirectKey, null);
          if (!string.IsNullOrEmpty(redirectUrl))
          {
            token.TokenResult = redirectUrl;
            result = true;
          }
          else
          {
            token.TokenError = "Mobile redirect url came back empty, make sure the token is inside a mobileRedirectRequired condition";
            token.TokenResult = string.Empty;
          }
        }
        else
        {
          token.TokenError = "Attribute \"redirectKey\" is required";
          token.TokenResult = string.Empty;
        }
      }
      else
      {
        token.TokenError = "Cannot convert IToken to MobileRedirectLinkToken.";
        token.TokenResult = string.Empty;
      }

      return result;
    }
  }
}
