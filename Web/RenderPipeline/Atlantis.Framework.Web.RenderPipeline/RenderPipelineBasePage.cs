using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class RenderPipelineBasePage : Page
  {
    private readonly RenderPipelineManager _renderPipelineManager = new RenderPipelineManager();

    private void RenderPage(HtmlTextWriter writer)
    {
      if (_renderPipelineManager.RenderHandlerCount > 0)
      {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
        {
          base.Render(htmlwriter);

          IRenderContent renderContent = new BasePageRenderContent(htmlwriter.InnerWriter.ToString());

          IProcessedRenderContent processedRenderContent = _renderPipelineManager.RenderContent(renderContent, HttpProviderContainer.Instance);

          writer.Write(processedRenderContent.Content);
        }
      }
      else
      {
        base.Render(writer);
      }
    }

    protected void AddRenderHandlers(params IRenderHandler[] renderHandlers)
    {
      _renderPipelineManager.AddRenderHandler(renderHandlers);
    }

    protected void AddRenderHandlers(IEnumerable<IRenderHandler> renderHandlers)
    {
      _renderPipelineManager.AddRenderHandler(renderHandlers.ToArray());
    }

    protected override void Render(HtmlTextWriter writer)
    {
      RenderPage(writer);
    }
  }
}
