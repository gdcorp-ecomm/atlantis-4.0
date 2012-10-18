using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPCustomPod: IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<HPCustomPodInfo> PodInfoList { get; set; }
    public HPCustomPod()
    {
      PodInfoList = new List<HPCustomPodInfo>();
    }
  }

  public class HPCustomPodInfo : IWidgetModel
  {
    public string CSS { get; set; }
    public string Content { get; set; }
    public string Script { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
