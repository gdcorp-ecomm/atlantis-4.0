using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class CDSPhraseHandler : ILanguagePhraseHandler
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILocalizationProvider> _localization;
    private const string _defaultLanguage = "en";

    public CDSPhraseHandler(IProviderContainer container)
    {
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);
    }

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      dictionaryName = dictionaryName.Substring(4);
      string phraseText;
      var fullLanguage = _localization.Value.FullLanguage;
      var fullLanguageDictionary = GetLanguageResponse(dictionaryName, fullLanguage);
      if (!TryGetPhraseText(fullLanguageDictionary, phraseKey, fullLanguage, out phraseText) && fullLanguage != _defaultLanguage)
      {
        var shortLanguage = _localization.Value.ShortLanguage;
        var shortLanguageDictionary = GetLanguageResponse(dictionaryName, shortLanguage);
        if (!TryGetPhraseText(shortLanguageDictionary, phraseKey, shortLanguage, out phraseText) && shortLanguage != _defaultLanguage)
        {
          var defaultLanguageDictionary = GetLanguageResponse(dictionaryName, _defaultLanguage);
          TryGetPhraseText(defaultLanguageDictionary, phraseKey, _defaultLanguage, out phraseText);
        }
      }
      return phraseText;
      
    }

    private static CDSLanguageResponseData GetLanguageResponse(string dictionaryName, string language)
    {
      var request = new CDSLanguageRequestData(dictionaryName, language);
      return (CDSLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.CDSLanguagePhrase);
    }

    private bool TryGetPhraseText(CDSLanguageResponseData response, string phraseKey, string language, out string phraseText)
    {
      var phrase = response.Phrases.FindPhrase(phraseKey, _siteContext.Value.ContextId, _localization.Value.CountrySite, language);
      phraseText = string.Empty;
      var exists = false;
      if (phrase != null)
      {
        exists = true;
        if (phrase.PhraseText != null)
        {
          phraseText = phrase.PhraseText;
        }
      }
      return exists;
    }
  }
}
