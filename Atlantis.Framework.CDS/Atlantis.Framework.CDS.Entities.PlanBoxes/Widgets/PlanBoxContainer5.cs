using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanBoxContainer5 : IWidgetModel
  {
    public string CiCode { get; set; }

    public string CrossSellConfig { get; set; }

    public string FormActionUrl { get; set; }

    public string FormName { get; set; }

    public bool FromDPP { get; set; }

    public bool HideInactiveAddOns { get; set; }

    public string ISC { get; set; }

    public string ItemTrackingCode { get; set; }

    public List<PlanBox6> PlanBoxes { get; set; }

    public bool RedirectStraightToCart { get; set; }
    
    public bool SkipDomainerCheck { get; set; }

    public bool UseNewCrossSellPage { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    public string XS_AppSettingKey { get; set; }

    public string XS_ContainerCode { get; set; }

    public string XS_LaunchCICode { get; set; }

  }
}