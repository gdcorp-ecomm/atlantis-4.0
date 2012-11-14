using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileAccordion : IWidgetModel
  {
    public string InlineStyles { get; set; }
    public List<MobileAccordDetails> Details { get; set; }
  }

  public class MobileAccordDetails
  {
    public string Text { get; set; }
    public List<MobileAccordSubDetails> SubDetails { get; set; }
  }

  public class MobileAccordSubDetails
  {
    public string InitialBoldText { get; set; }
    public string Text1 { get; set; }
    public string Text2 { get; set; }
  }
}
