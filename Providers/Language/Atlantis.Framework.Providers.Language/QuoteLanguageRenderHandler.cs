using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Providers.Language
{
  public class QuoteLanguageRenderHandler : LanguageRenderHandler
  {
    private const string LANGUAGE_TOKEN_PATTERN = @"\[@QL\[(?<dictionary>[a-zA-z0-9\.\-\/]*?):(?<phrasekey>[a-zA-z0-9\.\-]*?)\]@QL\]";

    public QuoteLanguageRenderHandler()
    {
      _languagePhraseTokenPattern = new Regex(LANGUAGE_TOKEN_PATTERN, RegexOptions.Singleline | RegexOptions.Compiled);
    }

    public override void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      var languageProvider = providerContainer.Resolve<ILanguageProvider>();
      IRenderPipelineStatus languageRenderStatus = null;
      IRenderPipelineStatusProvider statusProvider;

      if (providerContainer.TryResolve(out statusProvider))
      {
        languageRenderStatus = statusProvider.CreateNewStatus(RenderPipelineResult.Success, "QuoteLanguageRenderHandler");
      }

      const string doubleQuoteRegex = @"""";
      var modifiedContent = _languagePhraseTokenPattern.Replace(processRenderContent.Content, match => Regex.Replace(PhraseMatchEvaluator(match, languageProvider, languageRenderStatus), doubleQuoteRegex, "\\\""));
      
      if (languageRenderStatus != null)
      {
        statusProvider.Add(languageRenderStatus);
      }

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}