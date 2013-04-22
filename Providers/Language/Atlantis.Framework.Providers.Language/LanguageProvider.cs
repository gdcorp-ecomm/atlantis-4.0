using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;

namespace Atlantis.Framework.Providers.Language
{
  public class LanguageProvider : ProviderBase, ILanguageProvider
  {
    const string _DEFAULTCOUNTRYSITE = "www";
    const string _DEFAULTLANGUAGE = "en";

    Lazy<int> _contextId;
    Lazy<string> _language;
    Lazy<string> _countrySite;

    public LanguageProvider(IProviderContainer container)
      :base(container)
    {
      _contextId = new Lazy<int>(() => { return LoadContextId(); });
      _language = new Lazy<string>(() => { return LoadLanguage(); });
      _countrySite = new Lazy<string>(() => { return LoadCountrySite(); });
    }

    private string LoadCountrySite()
    {
      string result = _DEFAULTCOUNTRYSITE;
      ILocalizationProvider localization;
      if (Container.TryResolve(out localization))
      {
        result = localization.CountrySite;
      }
      return result;
    }

    private string LoadLanguage()
    {
      string result = _DEFAULTLANGUAGE;
      ILocalizationProvider localization;
      if (Container.TryResolve(out localization))
      {
        result = localization.FullLanguage;
      }
      return result;
    }

    private int LoadContextId()
    {
      var siteContext = Container.Resolve<ISiteContext>();
      return siteContext.ContextId;
    }

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dictionaryName, phraseKey, _language.Value, _countrySite.Value, _contextId.Value);
      var response = (LanguagePhraseResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.LanguagePhrase);
      return response.LanguagePhrase;
    }
  }
}
