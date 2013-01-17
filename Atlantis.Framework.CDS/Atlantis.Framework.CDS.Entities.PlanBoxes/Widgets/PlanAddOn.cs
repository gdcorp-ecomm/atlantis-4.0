using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.CDS.Entities.Widgets;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanAddOn : ElementBase
  {
    private List<int> _quantities;

    public PlanAddOn()
    {
      MinValue = 1;
      MaxValue = 999;
      DropDownItemDisplayMultiplier = 1;
      MatchPlanDuration = true;
    }

    [Range(0, int.MaxValue)]
    public int DropDownItemDisplayMultiplier { get; set; }

    public string DropDownItemDisplayText { get; set; }

    public bool Hide { get; set; }

    public InputType InputType { get; set; }

    public bool MatchPlanDuration { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxValue { get; set; }

    [Range(1, int.MaxValue)]
    public int MinValue { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Pfid { get; set; }

    public List<int> Quantities
    {
      get { return _quantities ?? (_quantities = new List<int>()); }
      set { _quantities = value; }
    }

  }
}

