using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class PlanBoxContainer2 : IWidgetModel
  {
    public string PlanBoxFormName { get; set; }
    public string FormActionUrl { get; set; }
    public string CiCode { get; set; }
    public string ItemTrackingCode { get; set; }
    public string ISC { get; set; }
    public string CrossSellConfig { get; set; }
    public string Upsell { get; set; }
    public bool RedirectStraightToCart { get; set; }
    public bool SkipDomainerCheck { get; set; }
    public bool UseNewCrossSellPage { get; set; }
    public bool FromDPP { get; set; }
    public bool HideInactiveAddOns { get; set; }
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }
}
