using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using Atlantis.Framework.CDS.Entities.Attributes;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Plan Boxes")]
  public class PlanBoxContainer : IWidgetModel
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
    public List<Widget<object>> Widgets { get; set; }
  }
}
