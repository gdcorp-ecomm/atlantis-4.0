using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Tests.RenderHandlers
{
  public class RenderHandlerThree : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      string contentToAppend = "three";

      if (!string.IsNullOrEmpty(processedRenderContent.Content))
      {
        contentToAppend = " " + contentToAppend;
      }

      processedRenderContent.OverWriteContent(processedRenderContent.Content + contentToAppend);
    }
  }
}
