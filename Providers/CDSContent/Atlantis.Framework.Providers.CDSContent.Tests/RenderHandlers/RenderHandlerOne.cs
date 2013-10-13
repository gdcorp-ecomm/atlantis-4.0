using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Tests.RenderHandlers
{
  public class RenderHandlerOne : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      string contentToAppend = "one";

      if (!string.IsNullOrEmpty(processedRenderContent.Content))
      {
        contentToAppend = " " + contentToAppend;
      }

      processedRenderContent.OverWriteContent(processedRenderContent.Content + contentToAppend);
    }
  }
}
