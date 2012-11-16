using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.ProductPlanBoxes.Widgets
{
  public class PackagerPlanBoxSimple : IWidgetModel
  {
    public string PackagerProductGroupId { get; set; }

    public string TierAttributeValue { get; set; }

    public int Width { get; set; }
    
    public bool DisplayBestValueFlag { get; set; }
    
    public bool ShowMonthlyView { get; set; }
    
    public string CustomHeaderDurationText { get; set; }

    public string AbovePlanDetailsCustomContent { get; set; }

    public List<PlanBox2.PlanDetail> PlanDetails { get; set; }

    public string AbovePlanFeaturesCustomContent { get; set; }

    public List<PlanBox2.PlanFeature> PlanFeatures { get; set; }

    public string BelowPlanFeaturesCustomContent { get; set; }

    public List<PlanAddOn> PlanAddOns { get; set; }

    public string InputType { get; set; }

    public string BelowPlanBoxCustomContent { get; set; }
  }

  public class PlanAddOn : ElementBase
  {
    public string InputLabel { get; set; }

    public string SelectType { get; set; }
    
    public string InputType { get; set; }

    public string AddOnName { get; set; }

    public bool Hide { get; set; }
  }
}
