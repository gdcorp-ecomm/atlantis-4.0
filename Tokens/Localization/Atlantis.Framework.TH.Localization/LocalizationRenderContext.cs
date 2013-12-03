using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.TH.Localization
{
  internal class LocalizationRenderContext
  {
    ILocalizationProvider _localizationProvider;

    internal LocalizationRenderContext(IProviderContainer container)
    {
      _localizationProvider = container.Resolve<ILocalizationProvider>();
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;
      LocalizationToken localizationToken = token as LocalizationToken;
      if (localizationToken != null)
      {
        switch (localizationToken.RenderType)
        {
          case "language":
            result = RenderLanguage(localizationToken);
            break;
          case "template":
            break;
          default:
            result = false;
            localizationToken.TokenError = "Invalid element root: " + localizationToken.RenderType;
            localizationToken.TokenResult = string.Empty;
            break;
        }
      }
      else
      {
        result = false;
        localizationToken.TokenError = "Cannot convert IToken to LocalizationToken";
        localizationToken.TokenResult = string.Empty;
      }
      return result;
    }

    private bool RenderLanguage(LocalizationToken token)
    {
      bool result = false;
      string language = String.Empty;
      if (token.FullLanguage)
      {
        language = _localizationProvider.FullLanguage;
      }
      else
      {
        language = _localizationProvider.ShortLanguage;
      }
      if (!String.IsNullOrEmpty(language))
      {
        result = true;
      }
      token.TokenResult = language;
      return result;
    }
  }
}
