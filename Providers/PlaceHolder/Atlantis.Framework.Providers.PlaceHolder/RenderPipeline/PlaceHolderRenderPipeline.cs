using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class PlaceHolderRenderPipeline
  {
    internal static string RunRenderPipeline(string content, IList<IRenderHandler> renderHandlers, IProviderContainer providerContainer)
    {
      string finalContent = content;

      if (renderHandlers != null)
      {
        RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
        renderPipelineManager.AddRenderHandler(renderHandlers);

        IRenderContent renderContent = new PlaceHolderRenderContent(content);
        IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, providerContainer);

        finalContent = processedRenderContent.Content;
      }

      return finalContent;
    }
  }
}
