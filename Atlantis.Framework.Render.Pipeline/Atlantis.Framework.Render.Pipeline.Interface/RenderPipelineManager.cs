using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.Pipeline.Interface
{
  public static class RenderPipelineManager
  {
    private static readonly RenderPipeline _renderPipeline = new RenderPipeline();

    public static void AddRenderHandler(IRenderHandler renderHandler)
    {
      _renderPipeline.Add(renderHandler);
    }

    public static void RenderContent(IRenderContent renderContent, IProviderContainer providerContainer)
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
                                                        providerContainer.Resolve<ISiteContext>(),
                                                        providerContainer.Resolve<IShopperContext>());

          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }
  }
}
