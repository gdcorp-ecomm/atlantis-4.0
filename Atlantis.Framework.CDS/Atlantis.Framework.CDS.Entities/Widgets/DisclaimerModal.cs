using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;


namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class DisclaimerModal : IWidgetModel
  {
    public int ProductGroup { get; set; }

    public class DisclaimerItem : ElementBase
    { }

    public List<DisclaimerItem> DisclaimerItems { get; set; }
    public bool Filtered { get; set; }
  }
}
