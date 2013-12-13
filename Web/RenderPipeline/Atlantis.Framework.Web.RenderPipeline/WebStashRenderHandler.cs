using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class WebStashRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = WebStashManager.ProcessScript(processRenderContent.Content);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
