using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Providers.CDS
{
  internal class CDSWidgetTokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent;
      
      ITokenEncoding cdsJsonEncoding = new CDSWidgetTokenEncoding();
      TokenManager.ReplaceTokens(processRenderContent.Content, providerContainer, cdsJsonEncoding, out modifiedContent);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
