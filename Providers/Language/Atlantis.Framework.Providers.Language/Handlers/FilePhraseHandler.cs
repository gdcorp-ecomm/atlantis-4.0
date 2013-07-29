using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Language.Interface;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  public class FilePhraseHandler : ILanguagePhraseHandler
  {
    public string GetLanguagePhrase(string dictionaryName, string phraseKey, LanguageData languageData)
    {
      var request = new LanguagePhraseRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dictionaryName, phraseKey, languageData.FullLanguage, languageData.CountrySite, languageData.ContextId);
      var response = (LanguagePhraseResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.FileLanguagePhrase);
      return response.LanguagePhrase;
    }
  }
}
