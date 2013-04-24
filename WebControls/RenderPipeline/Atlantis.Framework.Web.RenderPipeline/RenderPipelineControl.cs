using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.Pipeline.Interface;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.RenderPipeline
{
    [ToolboxData("<{0}:RenderPipeline1 runat=server></{0}:RenderPipeline1>")]
    public class RenderPipelineControl : PlaceHolder
    {
      private RenderPipelineManager _pipelineManager = null;

      public void AddRenderHandlers(params IRenderHandler[] renderHandlers)
      {
        if ((renderHandlers == null) || (renderHandlers.Length == 0))
        {
          return;
        }

        if (_pipelineManager == null)
        {
          _pipelineManager = new RenderPipelineManager();
        }

        _pipelineManager.AddRenderHandler(renderHandlers);
      }

      protected override void Render(HtmlTextWriter writer)
      {
        if (_pipelineManager == null)
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

        var content = new PipelineContent(prePipelineText.ToString());
        _pipelineManager.RenderContent(content, HttpProviderContainer.Instance);

        writer.Write(content.Content);
      }

      private class PipelineContent : IRenderContent
      {
        public PipelineContent(string content)
        {
          Content = content;
        }

        public string Content { get; set; }
      }
    }
}
