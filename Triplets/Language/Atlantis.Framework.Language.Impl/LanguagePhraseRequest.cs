using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Impl.Data;
using Atlantis.Framework.Language.Interface;

namespace Atlantis.Framework.Language.Impl
{
  public class LanguagePhraseRequest : IRequest
  {
    private static readonly LanguageData _languageData;
    private static readonly LanguagePhraseResponseData _emptyResponse;

    static LanguagePhraseRequest()
    {
      _languageData = new LanguageData();
      _emptyResponse = LanguagePhraseResponseData.FromPhrase(string.Empty);
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (LanguagePhraseRequestData)requestData;
      string phraseText = _languageData.FindPhrase(request.DictionaryName, request.PhraseKey, request.ContextId, request.CountrySite, request.FullLanguage);
      IResponseData result = !string.IsNullOrEmpty(phraseText) ? LanguagePhraseResponseData.FromPhrase(phraseText) : _emptyResponse;
      return result;
    }
  }
}
