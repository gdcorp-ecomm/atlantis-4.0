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
    const string _QALANGUAGESHOW = "qa-qa";

    Lazy<ISiteContext> _siteContext;
    Lazy<string> _language;
    Lazy<string> _countrySite;

    public LanguageProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
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

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      if (ShowDictionaryAndKeyRaw)
      {
        return GetQALanguagePhrase(dictionaryName, phraseKey);
      }
      else
      {
        var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dictionaryName, phraseKey, _language.Value, _countrySite.Value, _siteContext.Value.ContextId);
        var response = (LanguagePhraseResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.LanguagePhrase);
        return response.LanguagePhrase;
      }
    }

    private bool? _showDictionaryAndKeyRaw;
    private bool ShowDictionaryAndKeyRaw
    {
      get
      {
        if (!_showDictionaryAndKeyRaw.HasValue)
        {
          _showDictionaryAndKeyRaw = false;
          ILocalizationProvider localization;

          if ((_siteContext.Value.IsRequestInternal) && (Container.TryResolve(out localization)))
          {
            _showDictionaryAndKeyRaw = localization.IsActiveLanguage(_QALANGUAGESHOW);
          }
        }
        return _showDictionaryAndKeyRaw.Value;
      }
    }

    private string GetQALanguagePhrase(string dictionaryName, string phraseKey)
    {
      return string.Concat("[", dictionaryName, ":", phraseKey, "]");
    }

  }
}
