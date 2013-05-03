using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanBoxContainer4_noButton2 : IWidgetModel
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
    public string XS_ContainerCode { get; set; }
    public string XS_AppSettingKey { get; set; }
    public string XS_LaunchCICode { get; set; }
    
    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }
  }
}
