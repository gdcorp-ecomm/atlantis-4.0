using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  public class TokenRenderHandler : IRenderHandler
  {
    private ITokenEncoding _tokenEncoding;

    public TokenRenderHandler() : this(null)
    {
    }

    public TokenRenderHandler(ITokenEncoding tokenEncoding)
    {
      _tokenEncoding = tokenEncoding;
    }

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      TokenManager.ReplaceTokens(processRenderContent.Content, providerContainer, _tokenEncoding, out modifiedContent);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
