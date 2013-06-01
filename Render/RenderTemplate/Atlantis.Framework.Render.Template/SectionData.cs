using System;
using System.Text;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class SectionData
  {
    readonly StringBuilder _sectionDataBuilder = new StringBuilder();
    private string _name = string.Empty;

    private bool IsDataValid
    {
      get { return !string.IsNullOrEmpty(_name) && _sectionDataBuilder.Length > 0; }
    }

    internal SectionData(string name)
    {
      _name = name;
    }

    internal void Append(string content)
    {
      if (!string.IsNullOrEmpty(_name))
      {
        if(_sectionDataBuilder.Length == 0)
        {
          _sectionDataBuilder.Append(content);
        }
        else
        {
          _sectionDataBuilder.Append(Environment.NewLine + content);
        }
      }
    }

    internal bool Flush(out IRenderTemplateSection renderTemplateSection)
    {
      renderTemplateSection = null;

      if (IsDataValid)
      {
        renderTemplateSection = new RenderTemplateSection { Name = _name, Content = _sectionDataBuilder.ToString() };
        _name = null;
        _sectionDataBuilder.Clear();
      }

      return renderTemplateSection != null;
    }
  }
}
