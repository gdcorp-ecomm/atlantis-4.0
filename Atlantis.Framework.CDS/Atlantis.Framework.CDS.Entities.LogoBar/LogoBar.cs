using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class LogoBar : IWidgetModel
  {
    private IList<Icon> _icons;
    public IList<Icon> Icons
    {
      get { return _icons ?? (_icons = new List<Icon>()); }
      set { _icons = value; }
    }

    public string Title { get; set; }
    public string PopupActionText { get; set; }
    public string PopupAction { get; set; }

  }
}
