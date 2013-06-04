using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline.TestWeb.TestRenderHandlers
{
  public class SquigglyRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, Interface.IProviderContainer providerContainer)
    {
      processRenderContent.OverWriteContent(processRenderContent.Content.Replace("{{green}}", "KickAss!"));
    }
  }
}