using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Providers.Language
{
  public class LanguageRenderHandler : IRenderHandler
  {
    private const string DICTIONARY_GROUP_KEY = "dictionary";
    private const string PHRASE_GROUP_KEY = "phrasekey";
    private const string LANGUAGE_TOKEN_PATTERN = @"\[@L\[(?<dictionary>[a-zA-z0-9\.\-\/]*?):(?<phrasekey>[a-zA-z0-9\.\-]*?)\]@L\]";

    private static readonly Regex _languagePhraseTokenPattern = new Regex(LANGUAGE_TOKEN_PATTERN, RegexOptions.Singleline | RegexOptions.Compiled);

    private string PhraseMatchEvaluator(Match phraseMatch, ILanguageProvider languageProvider)
    {
      string dictionary = phraseMatch.Groups[DICTIONARY_GROUP_KEY].Captures[0].Value;
      string phrasekey = phraseMatch.Groups[PHRASE_GROUP_KEY].Captures[0].Value;

      return languageProvider.GetLanguagePhrase(dictionary, phrasekey);
    }

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      ILanguageProvider languageProvider = providerContainer.Resolve<ILanguageProvider>();

      string modifiedContent = _languagePhraseTokenPattern.Replace(processRenderContent.Content, match => PhraseMatchEvaluator(match, languageProvider));

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
