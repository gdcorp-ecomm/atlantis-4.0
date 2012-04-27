using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("Mini Plan Box")]
  public class OnePlanMini : IWidgetModel
  {
    public string ProductName { get; set; }
    public string ProductPrice { get; set; }
    public List<string> ListItems { get; set; }
    public bool IsYearly { get; set; }
    public int TabIndex { get; set; }
  }
}
