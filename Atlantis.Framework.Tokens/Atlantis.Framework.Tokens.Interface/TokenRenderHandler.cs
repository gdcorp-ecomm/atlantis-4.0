using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  public class TokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      TokenManager.ReplaceTokens(processedRenderContent.Content, providerContainer, out modifiedContent);

      processedRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
