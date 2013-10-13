using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Tests.RenderHandlers
{
  class RenderHandlerTwo : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      string contentToAppend = "two";

      if (!string.IsNullOrEmpty(processedRenderContent.Content))
      {
        contentToAppend = " " + contentToAppend;
      }

      processedRenderContent.OverWriteContent(processedRenderContent.Content + contentToAppend);
    }
  }
}
