using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ListPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<ListPodInfo> PodInfoList { get; set; }
    public ListPod()
    {
      PodInfoList = new List<ListPodInfo>();
    }
  }

  public class ListPodInfo : IWidgetModel
  {
    public string PodTitle { get; set; }
    public IList<ListPodListItem> PodListItems { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public string BottomLinkText { get; set; }
    public string BottomLink { get; set; }
    public ListPodInfo()
    {
      PodListItems = new List<ListPodListItem>();
    }
  }

  public class ListPodListItem : IWidgetModel
  {
    public string ItemText { get; set; }
    public string ItemLink { get; set; }
  }

}
