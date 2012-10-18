using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPBulkPanel : IWidgetModel
  {
    public string BackgroundTopColor { get; set; }
    public string BackgroundBottomColor { get; set; }
    public string HeaderText { get; set; }
    public string WaitMessage { get; set; }
    public string PromoDisclaimerText { get; set; }
    public string TeaserMessage { get; set; }
    public string SearchBoxText { get; set; }
    public IList<HPBulkLinkItem> BulkLinkItems { get; set; }
    public string TldsOffered { get; set; }

    public HPBulkPanel()
    {
      BulkLinkItems = new List<HPBulkLinkItem>();
    }
  }

  public class HPBulkLinkItem
  {
    public string LinkPosition { get; set; }
    public string LinkText { get; set; }
    public string Link { get; set; }
  }
}
