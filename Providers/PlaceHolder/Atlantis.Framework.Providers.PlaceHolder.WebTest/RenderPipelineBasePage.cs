using System.Web.UI;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public class RenderPipelineBasePage : Page
  {
    private readonly RenderPipelineManager _renderPipelineManager = new RenderPipelineManager();

    protected void AddRenderHandler(IRenderHandler renderHandler)
    {
      _renderPipelineManager.AddRenderHandler(renderHandler);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
      {
        base.Render(htmlwriter);
        string html = htmlwriter.InnerWriter.ToString();

        IRenderContent renderContent = new PageRenderContent(html);
        IProcessedRenderContent processedRenderContent = _renderPipelineManager.RenderContent(renderContent, HttpProviderContainer.Instance);

        writer.Write(processedRenderContent.Content);
      }
    }
  }
}