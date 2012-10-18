using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPPanelBulkSearch : IWidgetModel
  {
    public string BackgroundTopColor { get; set; }
    public string BackgroundBottomColor { get; set; }
    public string HeaderText { get; set; }
    public string WaitMessage { get; set; }
    public string PromoDisclaimerText { get; set; }
    public string TeaserMessage { get; set; }
    public string SearchBoxText { get; set; }
    public IList<HPBulkLinkItems> BulkLinkItems { get; set; }
    public string TldsOffered { get; set; }

    public HPPanelBulkSearch()
    {
      BulkLinkItems = new List<HPBulkLinkItems>();
    }
  }

  public class HPBulkLinkItems
  {
    public string LinkPosition { get; set; }
    public string LinkText { get; set; }
    public string Link { get; set; }
    public string CiCode { get; set; }
    public IList<HPBulkLinkParameter> BulkLinkParams { get; set; }
    public bool IsPopin { get; set; }
    public bool IsPanelSwitch { get; set; }

    public HPBulkLinkItems()
    {
      BulkLinkParams = new List<HPBulkLinkParameter>();
    }
  }

  public class HPBulkLinkParameter
  {
    public string Key { get; set; }
    public string Value { get; set; }
  }
}
