using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class OnePlanMini : IWidgetModel
  {
    public string ProductName { get; set; }
    public string ProductPrice { get; set; }
    public List<string> ListItems { get; set; }
    public bool IsYearly { get; set; }
    public int TabIndex { get; set; }
  }
}
