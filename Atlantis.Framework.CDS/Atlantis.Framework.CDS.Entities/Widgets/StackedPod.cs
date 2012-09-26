using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class StackedPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<PodInfo> PodInfoList { get; set; }

    public StackedPod()
    {
      PodInfoList = new List<PodInfo>();
    }
  }

  public class PodInfo : IWidgetModel
  {
    public string PodPosition { get; set; }
    public string PodLine1 { get; set; }
    public string PodLine2 { get; set; }
    public string PodLink { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
