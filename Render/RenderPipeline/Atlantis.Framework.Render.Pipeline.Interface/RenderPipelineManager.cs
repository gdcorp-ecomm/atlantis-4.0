using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.Pipeline.Interface
{
  public class RenderPipelineManager
  {
    private readonly RenderPipeline _renderPipeline = new RenderPipeline();

    public void AddRenderHandler(IRenderHandler renderHandler)
    {
      _renderPipeline.Add(renderHandler);
    }

    public void AddRenderHandler(params IRenderHandler[] renderHandlers)
    {
      foreach (IRenderHandler renderHandler in renderHandlers)
      {
        _renderPipeline.Add(renderHandler);  
      }
    }

    public IProcessedRenderContent RenderContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      IProcessedRenderContent processedRenderContent = new ProcessedRenderContent(renderContent);

      foreach (IRenderHandler renderHandler in _renderPipeline.RenderHandlers)
      {
        try
        {
          renderHandler.ProcessContent(processedRenderContent, providerContainer);
        }
        catch (Exception ex)
        {
          AtlantisException aex = new AtlantisException("RenderPipelineManager.ProcessRenderHandlers()",
                                                        "0",
                                                        string.Format("IRenderHandler error. Type: \"{0}\", Error: \"{1}\"", renderHandler.GetType(), ex.Message),
                                                        ex.StackTrace,
                                                        null,
                                                        null);

          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return processedRenderContent;
    }
  }
}
