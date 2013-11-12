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
        case "hosting":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Hosting);
          break;
        case "hostingexchange":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.HostingExchange);
          break;
        case "billing":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Billing);
          break;
        case "companyfax":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.CompanyFax);
          break;
        case "companymain":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.CompanyMain);
          break;
        case "domains":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Domains);
          break;
        case "premiumdomains":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.PremiumDomains);
          break;
        case "server":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Server);
          break;
        case "adspace":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.AdSpace);
          break;
        case "ssl":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.SSL);
          break;
        case "designteam":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.DesignTeam);
          break;
        case "resellersales":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.ResellerSales);
          break;
        case "mcafee":
          result = GetupportPhone(supportPhoneToken, SupportPhoneType.Mcafee);
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
