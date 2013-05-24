
namespace Atlantis.Framework.Render.Template.Interface
{
  public interface IRenderTemplateContent
  {
    IRenderTemplateData RenderTemplateData { get; }

    bool TryGetSection(string sectionName, out IRenderTemplateSection renderTemplateSection);
  }
}
