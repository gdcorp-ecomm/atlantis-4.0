using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.ProductPlanBoxes
{
  public class PackagerPlanAddOn : ElementBase
  {
    public string InputLabel { get; set; }

    public string InputType { get; set; }

    public string SelectType { get; set; }

    public string AddOnName { get; set; }

    public bool Hide { get; set; }
  }
}
