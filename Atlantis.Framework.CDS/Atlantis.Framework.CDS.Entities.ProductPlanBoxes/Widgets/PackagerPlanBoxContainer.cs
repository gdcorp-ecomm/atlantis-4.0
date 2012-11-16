using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.ProductPlanBoxes.Widgets
{
  public class PackagerPlanBoxContainer : IWidgetModel
  {
    public string CiCode { get; set; }

    public string ItemTrackingCode { get; set; }

    public string Isc { get; set; }

    public string CrossSellConfig { get; set; }

    public string Upsell { get; set; }

    public bool RedirectStraightToCart { get; set; }

    public bool SkipDomainerCheck { get; set; }

    public bool UseNewCrossSellPage { get; set; }

    public bool HideInactiveAddOns { get; set; }
    
    public string XsellContainerCode { get; set; }

    public string XsellAppSettingKey { get; set; }

    public string XsellLaunchCiCode { get; set; }
    
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }
}
