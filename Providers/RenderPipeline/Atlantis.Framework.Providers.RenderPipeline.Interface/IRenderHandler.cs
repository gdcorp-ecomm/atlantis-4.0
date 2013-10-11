using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IRenderHandler
  {
    void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer);
  }
}
