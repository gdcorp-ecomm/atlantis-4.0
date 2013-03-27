using Atlantis.Framework.CDS.Entities.Common;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanBox5_wButton1 : IWidgetModel
  {
    public string FormName { get; set; }
    public bool IsDefaultPlanBox { get; set; }
    public int BasePFID { get; set; }
    public int PlanHeadingPFID { get; set; }
    public string PlanPricePrefix { get; set; }
    public int Width { get; set; }
    public string Title { get; set; }
    public bool DisplayBestValueFlag { get; set; }
    public bool ShowMonthlyView { get; set; }
    public string CustomHeaderDurationText { get; set; }
    public bool RequireLogin { get; set; }
    public string RedirectModalText { get; set; }
    private string _submitButtonText;
    public string SubmitButtonText
    {
      get
      {
        if (string.IsNullOrEmpty(_submitButtonText))
        {
          _submitButtonText = "Add to Cart";
        }
        return _submitButtonText;
      }
      set
      {
        _submitButtonText = value;
      }
    }

    public List<PlanDetail> PlanDetails { get; set; }

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
      [Range(0, 100)]
      public int DiscountPercent { get; set; }
    }

    public bool UseQuantitySelector { get; set; }
    public int MaxQuantity { get; set; }

    public bool UseDurationSelector { get; set; }
    public int MinDuration { get; set; }
    public int MaxDuration { get; set; }
    public string DurationText { get; set; }

    public List<PlanAddOn> PlanAddOns { get; set; }

    public class PlanAddOn : ElementBase
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

    public List<PlanFeature> PlanFeatures { get; set; }

    public class PlanDetail : ElementBase { }
    public class PlanFeature : ElementBase { }

    public bool Filtered { get; set; }

    public string AbovePlanDetailsCustomContent { get; set; }

    [RegularExpression(@"^(\d+,)*\d+$", ErrorMessage="Custom quantities must be a comma-separated list of nonnegative integers")]
    public string CustomQuantities { get; set; }

    private int[] _customQuantitiesList;
    public int[] CustomQuantitiesList
    {
      get
      {
        if (_customQuantitiesList == null)
        {
          if (!string.IsNullOrEmpty(CustomQuantities))
          {
            string[] temp = CustomQuantities.Split(new char[] { ',' });
            _customQuantitiesList = Array.ConvertAll(temp, Convert.ToInt32);
          }
        }
        return _customQuantitiesList;
      }
    }

    public string BelowPlanBoxCustomContent { get; set; }
    public string AbovePlanFeaturesCustomContent { get; set; }
    public string BelowPlanFeaturesCustomContent { get; set; }

    [JsonIgnore]
    public List<Widget<IWidgetModel>> Widgets { get; set; }

    public string CustomPlanHeadingPriceText { get; set; }
  }
}
