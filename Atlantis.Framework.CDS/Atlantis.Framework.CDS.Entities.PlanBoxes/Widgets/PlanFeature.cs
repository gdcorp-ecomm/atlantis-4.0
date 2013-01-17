using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.PlanBoxes.Widgets
{
  public class PlanFeature : ElementBase
  {
    [Required]
    public string Title { get; set; }
  }
}

