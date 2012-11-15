using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileDomainSearchInBackImage : IWidgetModel
  {
    public string BackgroundImageUrl { get; set; }
    public string BackgroundImageWidth { get; set; }
    public string BackgroundImageHeight { get; set; }
    public string DomainSearchMarginTop { get; set; }
  }
}