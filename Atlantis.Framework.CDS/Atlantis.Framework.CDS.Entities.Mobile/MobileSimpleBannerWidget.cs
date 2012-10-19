using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileSimpleBannerWidget : IWidgetModel
  {
    public MobileSimpleBannerWidget()
    {
    }

    public string DefaultBanner { get; set; }
    public string PromoBanner { get; set; }
    public string[] PromoBannerCodeList { get; set; }

  }
}


