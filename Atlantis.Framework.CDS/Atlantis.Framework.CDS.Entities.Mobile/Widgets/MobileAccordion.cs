using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileAccordion : IWidgetModel
  {
    public List<MobileAccordDetails> Details { get; set; }
  }

  public class MobileAccordDetails : ElementBase
  {
    //public string Text { get; set; } -> comes from ElementBase
    public List<MobileAccordSubDetails> SubDetails { get; set; }
  }

  public class MobileAccordSubDetails : ElementBase
  {
    //public string Text { get; set; } -> comes from ElementBase
    public string Text2 { get; set; }
  }
}
