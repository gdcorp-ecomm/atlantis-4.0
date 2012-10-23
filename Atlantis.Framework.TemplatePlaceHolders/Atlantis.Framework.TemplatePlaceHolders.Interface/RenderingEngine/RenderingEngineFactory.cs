
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class RenderingEngineFactory
  {
    internal static IRenderingEngine GetInstance(string type)
    {
      IRenderingEngine renderingEngine;

      switch (type.ToLowerInvariant())
      {
        case "razor":
          renderingEngine = new RazorRenderingEngine();
          break;
        default:
          renderingEngine = new NullRenderingEngine();
          break;
      }

      return renderingEngine;
    }
  }
}
