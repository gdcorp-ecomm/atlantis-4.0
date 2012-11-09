using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileHome : IWidgetModel
  {
    public string ButtonText { get; set; }
    public string ButtonCssClass { get; set; }
    public List<MobileProductDisclaimerItem> DisclaimerList { get; set; }
  }
}
