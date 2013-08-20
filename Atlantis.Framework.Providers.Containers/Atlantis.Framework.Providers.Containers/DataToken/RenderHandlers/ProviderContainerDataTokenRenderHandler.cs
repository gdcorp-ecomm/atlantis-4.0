using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.Containers.DataToken.RenderHandlers
{
  public class ProviderContainerDataTokenRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = ProviderContainerDataTokenManager.ReplaceDataTokens(processRenderContent.Content, providerContainer);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
