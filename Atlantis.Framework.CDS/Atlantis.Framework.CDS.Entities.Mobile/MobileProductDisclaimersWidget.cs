using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileProductDisclaimersWidget : IWidgetModel
  {
    public List<MobileProductDisclaimerItem> DisclaimerList { get; set; }
  }

  public class MobileProductDisclaimerItem
  {
    public int ProductGroup { get; set; }
    public string Symbol { get; set; }
    public string DisclaimerText { get; set; }
  }
}