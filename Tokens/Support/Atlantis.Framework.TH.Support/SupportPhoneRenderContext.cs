using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Support
{
  internal class SupportPhoneRenderContext
  {
    readonly ISupportProvider _supportProvider;

    internal SupportPhoneRenderContext(IProviderContainer container)
    {
      _supportProvider = container.Resolve<ISupportProvider>();
    }

    internal bool RenderToken(IToken token)
    {
      bool result;

      SupportPhoneToken supportPhoneToken = (SupportPhoneToken) token;
      switch (supportPhoneToken.RenderType.ToLowerInvariant())
      {
        case "technical":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Technical);
          break;
        default:
          result = false;
          supportPhoneToken.TokenError = "SupportPhoneToken contains invalid RenderType";
          supportPhoneToken.TokenResult = string.Empty;
          break;
        }

      return result;
    }

    private bool GetupportPhone(SupportPhoneToken token, SupportPhoneType supportPhoneType)
    {
      bool result = false;

      string supportPhone = _supportProvider.GetSupportPhone(supportPhoneType).Number;

      if (!string.IsNullOrEmpty(supportPhone))
      {
        result = true;
      }

      token.TokenResult = supportPhone;
      return result;
    }
  }
}
