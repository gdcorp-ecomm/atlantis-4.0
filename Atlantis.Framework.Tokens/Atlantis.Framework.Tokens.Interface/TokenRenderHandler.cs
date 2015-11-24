using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  public class TokenRenderHandler : IRenderHandler
  {
    private readonly ITokenEncoding _tokenEncoding;

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
      providerContainer.Resolve<ITokenProvider>().ReplaceTokens(processRenderContent.Content, _tokenEncoding, out modifiedContent);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
