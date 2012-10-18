using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPStackedPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<HPPodInfo> PodInfoList { get; set; }

    public HPStackedPod()
    {
      PodInfoList = new List<HPPodInfo>();
    }
  }

  public class HPPodInfo : IWidgetModel
  {
    public string PodPosition { get; set; }
    public string PodLine1 { get; set; }
    public string PodLine2 { get; set; }
    public string PodLink { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
