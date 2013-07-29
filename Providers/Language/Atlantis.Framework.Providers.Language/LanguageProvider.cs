using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Handlers;
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

    readonly ISiteContext _siteContext;
    readonly ILocalizationProvider _localization;
    readonly bool _isInternal;
    readonly LanguageData _languageData = new LanguageData { ContextId = 0, CountrySite = _DEFAULTCOUNTRYSITE, DefaultLanguage = _DEFAULTLANGUAGE, FullLanguage = _DEFAULTLANGUAGE, ShortLanguage = _DEFAULTLANGUAGE };

    public LanguageProvider(IProviderContainer container)
      :base(container)
    {
      if (Container.TryResolve(out _siteContext))
      {
        _languageData.ContextId = _siteContext.ContextId;
        _isInternal = _siteContext.IsRequestInternal;
      }

      if(Container.TryResolve(out _localization)){
        _languageData.FullLanguage = _localization.FullLanguage;
        _languageData.ShortLanguage = _localization.ShortLanguage;
        _languageData.CountrySite = _localization.CountrySite;
      }
    }


    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {      
      ILanguagePhraseHandler handler = GetLanguagePhraseHandler(dictionaryName);
      return handler.GetLanguagePhrase(dictionaryName, phraseKey, _languageData);
    }
   
    private ILanguagePhraseHandler GetLanguagePhraseHandler(string dictionaryName)
    {
      if (ShowDictionaryAndKeyRaw)
      {
        return new QAPhraseHandler();
      }
      if (dictionaryName.StartsWith("cds.", StringComparison.OrdinalIgnoreCase))
      {
        return new CDSPhraseHandler();
      }
      return new FilePhraseHandler();
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

          if ((_isInternal) && (Container.TryResolve(out localization)))
          {
            _showDictionaryAndKeyRaw = localization.IsActiveLanguage(_QALANGUAGESHOW);
          }
        }
        return _showDictionaryAndKeyRaw.Value;
      }
    }
  }
}
