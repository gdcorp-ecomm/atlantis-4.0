using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileCommercials : IWidgetModel
  {
    public bool ShowToolBar { get; set; }
    public string CustomCommercialList { get; set; }
  }
}
