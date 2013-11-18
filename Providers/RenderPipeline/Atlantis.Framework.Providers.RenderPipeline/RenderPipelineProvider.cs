using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Threading;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  public class RenderPipelineProvider : ProviderBase, IRenderPipelineProvider
  {
    public RenderPipelineProvider(IProviderContainer container) : base(container)
    {
    }

    public string RenderContent(string content, IList<IRenderHandler> renderHandlers)
    {
      string finalContent;

      if (renderHandlers == null || string.IsNullOrEmpty(content))
      {
        finalContent = content;
      }
      else
      {
        try
        {
          IRenderContent renderContent = new RenderPipelineProviderContent(content);
          IProcessedRenderContent processedRenderContent = new ProcessedRenderContent(renderContent);

          foreach (IRenderHandler renderHandler in renderHandlers)
          {
            renderHandler.ProcessContent(processedRenderContent, Container);
          }

          finalContent = processedRenderContent.Content;
        }
        catch (ThreadAbortException)
        {
          finalContent = content;
        }
        catch (Exception ex)
        {
          finalContent = content;
          var exception = new AtlantisException("RenderPipelineProvider.RenderContent", 0, ex.ToString(), string.Empty);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return finalContent;
    }
  }
}