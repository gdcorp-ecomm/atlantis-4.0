using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.Language
{
  public class EncodedLanguageRenderHandler : LanguageRenderHandler
  {
    private const string LANGUAGE_TOKEN_PATTERN = @"\[@EL\[(?<dictionary>[a-zA-z0-9\.\-\/]*?):(?<phrasekey>[a-zA-z0-9\.\-]*?)\]@EL\]";

    public EncodedLanguageRenderHandler()
    {
      _languagePhraseTokenPattern = new Regex(LANGUAGE_TOKEN_PATTERN, RegexOptions.Singleline | RegexOptions.Compiled);
    }

    public override void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      ILanguageProvider languageProvider = providerContainer.Resolve<ILanguageProvider>();
      IRenderPipelineStatus languageRenderStatus = null;
      IRenderPipelineStatusProvider statusProvider;

      if (providerContainer.TryResolve(out statusProvider))
      {
        languageRenderStatus = statusProvider.CreateNewStatus(RenderPipelineResult.Success, "EncodedLanguageRenderHandler");
      }

      string modifiedContent = _languagePhraseTokenPattern.Replace(processRenderContent.Content, match => HttpUtility.HtmlEncode(PhraseMatchEvaluator(match, languageProvider, languageRenderStatus)));

      if (languageRenderStatus != null)
      {
        statusProvider.Add(languageRenderStatus);
      }

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}