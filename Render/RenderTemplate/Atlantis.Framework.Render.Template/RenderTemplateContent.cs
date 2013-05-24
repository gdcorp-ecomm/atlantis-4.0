using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class RenderTemplateContent : IRenderTemplateContent
  {
    private readonly IRenderContent _renderContent;
    
    private RenderTemplateSectionData _renderTemplateSectionData;
    private RenderTemplateSectionData RenderTemplateSectionData
    {
      get { return _renderTemplateSectionData ?? (_renderTemplateSectionData = new RenderTemplateSectionData(_renderContent)); }
    }

    private IRenderTemplateData _renderTemplateData;
    public IRenderTemplateData RenderTemplateData
    {
      get { return _renderTemplateData ?? (_renderTemplateData = new RenderTemplateData(_renderContent)); }
    }

    public bool TryGetSection(string sectionName, out IRenderTemplateSection renderTemplateSection)
    {
      return RenderTemplateSectionData.TryGetSection(sectionName, out renderTemplateSection);
    }

    internal RenderTemplateContent(IRenderContent renderContent)
    {
      _renderContent = renderContent;
    }
  }
}
