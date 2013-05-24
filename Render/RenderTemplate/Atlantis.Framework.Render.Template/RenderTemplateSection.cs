using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class RenderTemplateSection : IRenderTemplateSection
  {
    public string Name { get; internal set; }

    public string Content { get; internal set; }
  }
}
