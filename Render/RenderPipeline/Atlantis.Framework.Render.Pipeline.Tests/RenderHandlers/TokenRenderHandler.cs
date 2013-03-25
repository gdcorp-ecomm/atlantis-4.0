using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers
{
  public class TokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      TokenEvaluationResult tokenEvaluationResult = TokenManager.ReplaceTokens(renderContent.Content, providerContainer, out modifiedContent);

      renderContent.Content = modifiedContent;
    }
  }
}
