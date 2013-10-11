using System.Collections.Generic;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.RenderPipeline
{
  [ToolboxData("<{0}:RenderPipeline1 runat=server></{0}:RenderPipeline1>")]
  public class RenderPipelineControl : PlaceHolder
  {
    private readonly List<IRenderHandler> _renderHandlers = new List<IRenderHandler>(32);

    public void AddRenderHandlers(params IRenderHandler[] renderHandlers)
    {
      if ((renderHandlers == null) || (renderHandlers.Length == 0))
      {
        return;
      }

      _renderHandlers.AddRange(renderHandlers);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (_renderHandlers.Count == 0)
      {
        base.Render(writer);
      }
      else
      {
        PipelineRender(writer);
      }
    }

    private void PipelineRender(HtmlTextWriter writer)
    {
      StringBuilder prePipelineText = new StringBuilder(1000);

      using (var prePipelineStringWriter = new StringWriter(prePipelineText))
      {
        using (var prePipelineHtml = new HtmlTextWriter(prePipelineStringWriter))
        {
          base.Render(prePipelineHtml);
        }
      }

      IRenderPipelineProvider renderPipelineProvider = HttpProviderContainer.Instance.Resolve<IRenderPipelineProvider>();

      string renderedContent = renderPipelineProvider.RenderContent(prePipelineText.ToString(), _renderHandlers);

      writer.Write(renderedContent);
    }
  }
}
