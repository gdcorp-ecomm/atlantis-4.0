using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers
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
