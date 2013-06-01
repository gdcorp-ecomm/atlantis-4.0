using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal static class RenderTemplateHandlerFactory
  {
    internal static IRenderTemplateHandler GetHandler(IRenderTemplateContent renderTemplateContent)
    {
      IRenderTemplateHandler renderTemplateHandler;

      switch (renderTemplateContent.RenderTemplateData.Type.ToLowerInvariant())
      {
        case "masterpage":
          renderTemplateHandler = new MasterPageRenderTemplateHandler(renderTemplateContent);
          break;
        default:
          renderTemplateHandler = new NullRenderTemplateHandler(renderTemplateContent);
          break;
      }

      return renderTemplateHandler;
    }
  }
}
