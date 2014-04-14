using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Providers.Language
{
  public class LanguageRenderHandler : IRenderHandler
  {
    private const string DICTIONARY_GROUP_KEY = "dictionary";
    private const string PHRASE_GROUP_KEY = "phrasekey";
    private const string LANGUAGE_TOKEN_PATTERN = @"\[@L\[(?<dictionary>[a-zA-z0-9\.\-\/]*?):(?<phrasekey>[a-zA-z0-9\.\-]*?)\]@L\]";

    protected Regex _languagePhraseTokenPattern = new Regex(LANGUAGE_TOKEN_PATTERN, RegexOptions.Singleline | RegexOptions.Compiled);

    protected string PhraseMatchEvaluator(Match phraseMatch, ILanguageProvider languageProvider, IRenderPipelineStatus languageRenderStatus)
    {
      string dictionary = phraseMatch.Groups[DICTIONARY_GROUP_KEY].Captures[0].Value;
      string phrasekey = phraseMatch.Groups[PHRASE_GROUP_KEY].Captures[0].Value;

      string phrase;
      if (!languageProvider.TryGetLanguagePhrase(dictionary, phrasekey, out phrase))
      {
        phrase = string.Empty;

        if (languageRenderStatus != null)
        {
          languageRenderStatus.Status = RenderPipelineResult.SuccessWithErrors;
          languageRenderStatus.AddData("Phrase key not found", phraseMatch.ToString());
        }
      }

      return phrase;
    }

    public virtual void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      ILanguageProvider languageProvider = providerContainer.Resolve<ILanguageProvider>();
      IRenderPipelineStatus languageRenderStatus = null;
      IRenderPipelineStatusProvider statusProvider;
      
      if (providerContainer.TryResolve(out statusProvider))
      {
        languageRenderStatus = statusProvider.CreateNewStatus(RenderPipelineResult.Success, "LanguageRenderHandler");
      }

      string modifiedContent = _languagePhraseTokenPattern.Replace(processRenderContent.Content, match => PhraseMatchEvaluator(match, languageProvider, languageRenderStatus));

      if (languageRenderStatus != null && statusProvider != null)
      {
        statusProvider.Add(languageRenderStatus);
      }

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
