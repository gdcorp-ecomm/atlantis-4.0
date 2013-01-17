using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.CDS.Entities.Widgets;
using Newtonsoft.Json;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanProduct : ElementBase
  {
    [Range(0, 100)]
    public int DiscountPercent { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Duration { get; set; }

    public bool IsDefault
    {
      get;
      set;
    }

    [Required]
    [Range(1, int.MaxValue)]
    public int PfId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

  }
}

