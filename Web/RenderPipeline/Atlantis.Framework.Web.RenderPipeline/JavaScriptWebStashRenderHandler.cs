using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class JavaScriptWebStashRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = JavaScriptWebStashManager.Replace(processRenderContent.Content);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
