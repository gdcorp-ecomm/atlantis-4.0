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

    public void RenderContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      foreach (IRenderHandler renderHandler in _renderPipeline.RenderHandlers)
      {
        try
        {
          renderHandler.ProcessContent(renderContent, providerContainer);
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
    }
  }
}
