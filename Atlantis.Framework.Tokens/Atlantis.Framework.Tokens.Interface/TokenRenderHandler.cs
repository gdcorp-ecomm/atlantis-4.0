using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  public class TokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      TokenManager.ReplaceTokens(renderContent.Content, providerContainer, out modifiedContent);

      renderContent.Content = modifiedContent;
    }
  }
}
