using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  public static class RenderTemplateManager
  {
    public static IRenderTemplateContent ParseTemplateContent(IRenderContent renderContent)
    {
      return new RenderTemplateContent(renderContent);
    }

    public static void ProcessTemplate(IRenderTemplateContent renderTemplateContent)
    {
      IRenderTemplateHandler renderTemplateHandler = RenderTemplateHandlerFactory.GetHandler(renderTemplateContent);
      renderTemplateHandler.ProcessTemplate();
    }
  }
}
