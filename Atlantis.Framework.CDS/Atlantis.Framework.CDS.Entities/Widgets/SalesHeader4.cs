using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SalesHeader4 : SalesHeader3, IContent, IAddToCartWidgetModel
  {
    public SalesHeader4()
    {
      this.Buttons = new List<Widgets.MarketingButton>();
      this.ProductOptions = new List<ProductOption>();
    }

    public List<Widgets.MarketingButton> Buttons { get; set; }
    public string CiCode { get; set; }
    public string CrossSellConfig { get; set; }
    public string FormName { get; set; }
    public string FormActionUrl { get; set; }
    public bool FromDPP { get; set; }
    public string ISC { get; set; }
    public string ItemTrackingCode { get; set; }
    public List<ProductOption> ProductOptions { get; set; }
    public bool RedirectStraightToCart { get; set; }
    public bool SkipDomainerCheck { get; set; }
    public string Upsell { get; set; }
    public bool UseNewCrossSellPage { get; set; }
    public string XS_AppSettingKey { get; set; }
    public string XS_ContainerCode { get; set; }
    public string XS_LaunchCICode { get; set; }

  }
}
