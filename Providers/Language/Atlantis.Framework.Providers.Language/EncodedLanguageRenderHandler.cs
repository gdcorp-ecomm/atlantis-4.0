using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.Language
{
  public class EncodedLanguageRenderHandler : IRenderHandler
  {
    private static readonly Regex _languagePhraseTokenPattern = new Regex("\\[@EL\\[(?<dictionary>[a-zA-z0-9\\.\\-\\/]*?):(?<phrasekey>[a-zA-z0-9\\.\\-]*?)\\]@EL\\]", RegexOptions.Compiled | RegexOptions.Singleline);
    private const string DICTIONARY_GROUP_KEY = "dictionary";
    private const string PHRASE_GROUP_KEY = "phrasekey";

    static EncodedLanguageRenderHandler()
    {
    }

    private string PhraseMatchEvaluator(Match phraseMatch, ILanguageProvider languageProvider)
    {
      var dictionaryName = phraseMatch.Groups[DICTIONARY_GROUP_KEY].Captures[0].Value;
      var phraseKey = phraseMatch.Groups[PHRASE_GROUP_KEY].Captures[0].Value;
      return HttpUtility.HtmlEncode(languageProvider.GetLanguagePhrase(dictionaryName, phraseKey));
    }

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      var languageProvider = providerContainer.Resolve<ILanguageProvider>();
      var content = _languagePhraseTokenPattern.Replace(processRenderContent.Content, match => PhraseMatchEvaluator(match, languageProvider));
      processRenderContent.OverWriteContent(content);
    }
  }
}