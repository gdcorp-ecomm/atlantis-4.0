using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Impl.Data;
using Atlantis.Framework.Language.Interface;

namespace Atlantis.Framework.Language.Impl
{
  public class LanguagePhraseRequest : IRequest
  {
    private static LanguageData _languageData;
    private static LanguagePhraseResponseData _emptyResponse;

    static LanguagePhraseRequest()
    {
      _languageData = new LanguageData();
      _emptyResponse = LanguagePhraseResponseData.FromPhrase(string.Empty);
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;
      LanguagePhraseRequestData request = (LanguagePhraseRequestData)requestData;
      string phraseText = _languageData.FindPhrase(request.DictionaryName, request.PhraseKey, request.ContextId, request.CountrySite, request.FullLanguage);

      if (!string.IsNullOrEmpty(phraseText))
      {
        result = LanguagePhraseResponseData.FromPhrase(phraseText);
      }
      else
      {
        result = _emptyResponse;
      }

      return result;
    }
  }
}
