using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipeline.TestWeb.TestRenderHandlers
{
  public class SquigglyRenderHandler : IRenderHandler
  {
    public void ProcessContent(IRenderContent renderContent, Interface.IProviderContainer providerContainer)
    {
      renderContent.Content = renderContent.Content.Replace("{{green}}", "KickAss!");
    }
  }
}