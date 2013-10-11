using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests.Helpers
{
  public class TestRenderHandler : IRenderHandler
  {
    public string ContentToReturn { get; set; }
    public void ProcessContent(IProcessedRenderContent processedRenderContent, Framework.Interface.IProviderContainer providerContainer)
    {
      ContentToReturn = "test";
      processedRenderContent.OverWriteContent(ContentToReturn);
    }
  }
}
