using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class LogoBar : IWidgetModel
  {
    public enum IconType
    {
      PHP,
      DotNet,
      Ruby,
      Perl,
      Python,
      WordPress,
      Joomla,
      Drupal,
      DotNetNuke
    }

    public string Title { get; set; }
    public string PopupAction { get; set; }
    private IList<IconType> _icons;
    public IList<IconType> Icons
    {
      get { return _icons ?? (_icons = new List<IconType>()); }
      set { _icons = value; }
    }
  }
}
