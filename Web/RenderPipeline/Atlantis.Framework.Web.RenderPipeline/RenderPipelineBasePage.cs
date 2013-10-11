using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public abstract class RenderPipelineBasePage : Page
  {
    private readonly List<IRenderHandler> _renderHandlers = new List<IRenderHandler>(32); 

    private void RenderPage(HtmlTextWriter writer)
    {
      if (_renderHandlers.Count > 0)
      {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
        {
          base.Render(htmlwriter);

          IRenderPipelineProvider renderPipelineProvider = HttpProviderContainer.Instance.Resolve<IRenderPipelineProvider>();

          string renderedContent = renderPipelineProvider.RenderContent(htmlwriter.InnerWriter.ToString(), _renderHandlers);

          writer.Write(renderedContent);
        }
      }
      else
      {
        base.Render(writer);
      }
    }

    protected void AddRenderHandlers(params IRenderHandler[] renderHandlers)
    {
      _renderHandlers.AddRange(renderHandlers);
    }

    protected void AddRenderHandlers(IEnumerable<IRenderHandler> renderHandlers)
    {
      _renderHandlers.AddRange(renderHandlers);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      RenderPage(writer);
    }
  }
}
