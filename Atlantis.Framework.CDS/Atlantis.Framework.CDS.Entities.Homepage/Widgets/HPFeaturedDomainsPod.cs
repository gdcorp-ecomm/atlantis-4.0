using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPFeaturedDomainsPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<HPFeaturedDomainsPodInfo> PodInfoList { get; set; }
    public HPFeaturedDomainsPod()
    {
      PodInfoList = new List<HPFeaturedDomainsPodInfo>();
    }
  }

  public class HPFeaturedDomainsPodInfo : IWidgetModel
  {
    public string PodPosition { get; set; }
    public string IconImage { get; set; }
    public string ImagePositionTop { get; set; }
    public string ImagePositionLeft { get; set; }
    public string ImageHeight { get; set; }
    public string ImageWidth { get; set; }
    public string PodText { get; set; }
    public string PodLink { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}

