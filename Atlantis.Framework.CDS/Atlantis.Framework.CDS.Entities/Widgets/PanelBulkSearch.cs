using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class PanelBulkSearch : IWidgetModel
  {
    public string BackgroundTopColor { get; set; }
    public string BackgroundBottomColor { get; set; }
    public string HeaderText { get; set; }
    public string WaitMessage { get; set; }
    public string PromoDisclaimerText { get; set; }
    public string TeaserMessage { get; set; }
    public string SearchBoxText { get; set; }
    public IList<BulkLinkItem> BulkLinkItems { get; set; }
    public string TldsOffered { get; set; }

    public PanelBulkSearch()
    {
      BulkLinkItems = new List<BulkLinkItem>();
    }
  }

  public class BulkLinkItem
  {
    public string LinkPosition { get; set; }
    public string LinkText { get; set; }
    public string Link { get; set; }
    public string CiCode { get; set; }
    public IList<BulkLinkParameter> BulkLinkParams { get; set; }
    public bool IsPopin { get; set; }
    public bool IsPanelSwitch { get; set; }

    public BulkLinkItem()
    {
      BulkLinkParams = new List<BulkLinkParameter>();
    }
  }

  public class BulkLinkParameter
  {
    public string Key { get; set; }
    public string Value { get; set; }
  }
}
