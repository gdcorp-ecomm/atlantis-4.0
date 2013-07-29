using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Language.Interface;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  class CDSPhraseHandler : ILanguagePhraseHandler
  {
    public string GetLanguagePhrase(string dictionaryName, string phraseKey, LanguageData languageData)
    {
      PhraseDictionaryResult dictionaryResult;
      dictionaryName = dictionaryName.Substring(4);
      var phraseText = string.Empty;
      if (!GetPhraseDictionary(dictionaryName, languageData.FullLanguage, out dictionaryResult))
      {
        if (!GetPhraseDictionary(dictionaryName, languageData.ShortLanguage, out dictionaryResult))
        {
          GetPhraseDictionary(dictionaryName, languageData.DefaultLanguage, out dictionaryResult);
        }
      }
      if (dictionaryResult != null)
      {
        var phrase = dictionaryResult.Dictionary.FindPhrase(phraseKey, languageData.ContextId, languageData.CountrySite, dictionaryResult.CurrentLanguage);
        if (phrase != null)
        {
          phraseText = phrase.PhraseText;
        }
      }
      return phraseText;
    }

    private static bool GetPhraseDictionary(string dictionaryName, string language, out PhraseDictionaryResult phraseDictionaryResult)
    {
      var hasDictionary = false;
      phraseDictionaryResult = new PhraseDictionaryResult();

      var request = new CDSLanguageRequestData { DictionaryName = dictionaryName, Language = language, };
      var response = (CDSLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.CDSLanguagePhrase);

      if (response.GetException() == null)
      {
        phraseDictionaryResult.Dictionary = response.Phrases;
        phraseDictionaryResult.CurrentLanguage = language;
        hasDictionary = phraseDictionaryResult.Dictionary.phraseGroups.Count > 0;
      }
      return hasDictionary;
    }
  }
}
