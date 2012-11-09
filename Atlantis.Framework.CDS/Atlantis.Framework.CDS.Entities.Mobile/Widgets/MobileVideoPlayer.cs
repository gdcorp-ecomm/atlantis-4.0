using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileVideoPlayer : IWidgetModel
  {
    public bool HideVideoHeader { get; set; }
    public string HeaderContainerId { get; set; }
  }
}
