using System;
using System.Collections.Generic;
using System.Threading;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  public class RenderPipelineProvider : ProviderBase, IRenderPipelineProvider
  {
    public RenderPipelineProvider(IProviderContainer container) : base(container)
    {

    }

    public string RenderContent(string content, IList<IRenderHandler> renderHandlers, IProviderContainer providerContainer)
    {
      string finalContent;

      if (renderHandlers == null || providerContainer == null || string.IsNullOrEmpty(content))
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
            renderHandler.ProcessContent(processedRenderContent, providerContainer);
          }

          finalContent = processedRenderContent.Content;
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