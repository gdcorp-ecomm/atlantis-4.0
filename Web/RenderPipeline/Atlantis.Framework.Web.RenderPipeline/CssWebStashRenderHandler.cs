using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class CssWebStashRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = CssWebStashManager.ProcessCss(processRenderContent.Content);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
