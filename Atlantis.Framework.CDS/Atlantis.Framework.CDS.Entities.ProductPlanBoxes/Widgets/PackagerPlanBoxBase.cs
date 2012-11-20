using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.ProductPlanBoxes.Widgets
{
  public abstract class PackagerPlanBoxBase : IWidgetModel
  {
    public bool Filtered { get; set; }

    public string TierAttributeValue { get; set; }

    public int Width { get; set; }

    public bool DisplayBestValueFlag { get; set; }

    public string PlanPricePrefix { get; set; }

    public bool ShowMonthlyView { get; set; }

    public string CustomHeaderDurationText { get; set; }

    public List<PlanBox2.PlanDetail> PlanDetails { get; set; }

    public List<PlanBox2.PlanFeature> PlanFeatures { get; set; }

    public List<PackagerPlanAddOn> PlanAddOns { get; set; }
  }
}
