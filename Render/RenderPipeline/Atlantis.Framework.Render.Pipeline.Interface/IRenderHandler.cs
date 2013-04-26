using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.Pipeline.Interface
{
  public interface IRenderHandler
  {
    void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer);
  }
}
