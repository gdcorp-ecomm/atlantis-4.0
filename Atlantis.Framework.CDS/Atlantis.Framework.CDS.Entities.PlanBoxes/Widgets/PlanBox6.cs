using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanBox6 : IWidgetModel
  {
    private List<PlanAddOn> _addOns;
    private List<PlanDetail> _details;
    private List<PlanFeature> _features;
    private List<PlanProduct> _products;

    public List<PlanAddOn> AddOns
    {
      get { return _addOns ?? (_addOns = new List<PlanAddOn>()); }
      set { _addOns = value; }
    }

    [Required]
    [Range(1, int.MaxValue)]
    public int BasePfId { get; set; }

    public string CustomHeaderDurationText { get; set; }

    public string CustomPriceHeadingText { get; set; }

    public List<PlanDetail> Details
    {
      get { return _details ?? (_details = new List<PlanDetail>()); }
      set { _details = value; }
    }
    public string DurationText { get; set; }

    public List<PlanFeature> Features
    {
      get { return _features ?? (_features = new List<PlanFeature>()); }
      set { _features = value; }
    }

    public string FeatureTitle { get; set; }

    public bool Filtered { get; set; }

    public InputType ProductSelectionType { get; set; }

    public bool IsBestValue { get; set; }

    public bool IsDefault { get; set; }

    public int MinDuration { get; set; }
    public int MaxDuration { get; set; }

    public string PricePrefix { get; set; }

    public List<PlanProduct> Products
    {
      get { return _products ?? (_products = new List<PlanProduct>()); }
      set { _products = value; }
    }

    public bool ShowMonthlyView { get; set; }

    public string Title { get; set; }

    public string Upsell { get; set; }

    public bool UseDurationSelector { get; set; }

    public bool UseNewCrossSellPage { get; set; }

    public bool UseQuantitySelector { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    public int Width { get; set; }

  }
}

