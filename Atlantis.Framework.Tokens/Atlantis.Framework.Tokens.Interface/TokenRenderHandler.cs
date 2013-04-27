using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  public class TokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      TokenManager.ReplaceTokens(processRenderContent.Content, providerContainer, out modifiedContent);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
