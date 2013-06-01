using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class NullRenderTemplateHandler : IRenderTemplateHandler
  {
    private readonly IRenderTemplateContent _renderTemplateContent;

    internal NullRenderTemplateHandler(IRenderTemplateContent renderTemplateContent)
    {
      _renderTemplateContent = renderTemplateContent;
    }

    public void ProcessTemplate()
    {
      AtlantisException aex = new AtlantisException("ProcessTemplate()",
                                                     "0",
                                                     "Unknown Render Template Type",
                                                     "Render Template Type: " + _renderTemplateContent.RenderTemplateData.Type,
                                                     null,
                                                     null); 

      Engine.Engine.LogAtlantisException(aex);
    }
  }
}
