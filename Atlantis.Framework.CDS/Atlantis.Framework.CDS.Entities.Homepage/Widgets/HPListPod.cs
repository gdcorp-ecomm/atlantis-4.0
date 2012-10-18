using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPListPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<HPListPodInfo> PodInfoList { get; set; }
    public HPListPod()
    {
      PodInfoList = new List<HPListPodInfo>();
    }
  }

  public class HPListPodInfo : IWidgetModel
  {
    public string PodTitle { get; set; }
    public IList<HPListPodListItem> PodListItems { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public string BottomLinkText { get; set; }
    public string BottomLink { get; set; }
    public HPListPodInfo()
    {
      PodListItems = new List<HPListPodListItem>();
    }
  }

  public class HPListPodListItem : IWidgetModel
  {
    public string ItemText { get; set; }
    public string ItemLink { get; set; }
  }

}
