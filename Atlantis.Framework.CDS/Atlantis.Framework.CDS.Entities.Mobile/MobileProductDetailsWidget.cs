using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileProductDetailsWidget : IWidgetModel
  {
    public int ProductGroupId { get; set; }
    public string IconUrl { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string BodyHeader { get; set; }
    public string BodyText { get; set; }
    public string ListHeader { get; set; }
    public List<MobileProductDetailListItem> ListItems { get; set; }
    public bool ShowIcon { get; set; }
    public bool ShowTitle { get; set; }
    public bool ShowSubTitle { get; set; }
    public bool ShowBodyHeader { get; set; }
    public bool ShowBodyText { get; set; }
    public bool ShowList { get; set; }
    public bool ShowListHeader { get; set; }
  }

  public class MobileProductDetailListItem : ElementBase
  {
  }
}