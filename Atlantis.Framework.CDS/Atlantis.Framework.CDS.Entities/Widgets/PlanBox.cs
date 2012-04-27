using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Plan Box")]
  public class PlanBox : IWidgetModel
  {
    public PlanBox()
    {
    }

    public string FormName { get; set; }
    public bool IsDefaultPlanBox { get; set; }
    public int BasePFID { get; set; }
    public int Width { get; set; }
    public string Title { get; set; }
    public bool DisplayBestValueFlag { get; set; }
    public bool ShowMonthlyView { get; set; }
    public string CustomHeaderDurationText { get; set; }

    public List<string> PlanDetails { get; set; }

    public string InputType { get; set; }
    public int DefaultSelectionPFID { get; set; }

    private int? _defaultSelectionIndex;
    public int? DefaultSelectionIndex
    {
      get
      {
        if (!_defaultSelectionIndex.HasValue)
        {
          _defaultSelectionIndex = -1;
        }
        return _defaultSelectionIndex.Value;
      }
      set
      {
        _defaultSelectionIndex = value;
      }
    }

    public List<ProductOption> ProductList { get; set; }

    public class ProductOption : ElementBase
    {
      public int ProductId { get; set; }
      public int Quantity { get; set; }
      public int Duration { get; set; }
    }

    public bool UseQuantitySelector { get; set; }
    public int MaxQuantity { get; set; }

    public bool UseDurationSelector { get; set; }
    public int MinDuration { get; set; }
    public int MaxDuration { get; set; }
    public string DurationText { get; set; }

    public List<PlanAddOn> PlanAddOns { get; set; }

    public class PlanAddOn
    {
      public PlanAddOn()
      {
      }

      public bool Hide { get; set; }
      public string SelectType { get; set; }
      public string InputLabel { get; set; }
      public string InputType { get; set; }

      private int? _minValue;
      public int? MinValue
      {
        get
        {
          if (!_minValue.HasValue)
          {
            _minValue = 1;
          }
          return _minValue.Value;
        }
        set
        {
          _minValue = value;
        }
      }

      private int? _maxValue;
      public int? MaxValue
      {
        get
        {
          if (!_maxValue.HasValue)
          {
            _maxValue = 999;
          }
          return _maxValue.Value;
        }
        set
        {
          _maxValue = value;
        }
      }
      public int ProductId { get; set; }
      public List<int> DropDownQuantities { get; set; }

      private int? _dropDownItemDisplayMultiplier;
      public int? DropDownItemDisplayMultiplier
      {
        get
        {
          if (!_dropDownItemDisplayMultiplier.HasValue)
          {
            _dropDownItemDisplayMultiplier = 1;
          }
          return _dropDownItemDisplayMultiplier.Value;
        }
        set
        {
          _dropDownItemDisplayMultiplier = value;
        }
      }

      public string DropDownItemDisplayText { get; set; }
      public string TextBoxLabel { get; set; }

      private bool? _matchPlanDuration;
      public bool? MatchPlanDuration
      {
        get
        {
          if (!_matchPlanDuration.HasValue)
          {
            _matchPlanDuration = true;
          }
          return _matchPlanDuration.Value;
        }
        set
        {
          _matchPlanDuration = value;
        }
      }
    }

    public List<string> PlanFeatures { get; set; }
  }
}
