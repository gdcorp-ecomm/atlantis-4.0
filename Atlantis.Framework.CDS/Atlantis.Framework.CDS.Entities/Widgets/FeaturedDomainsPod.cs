using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class FeaturedDomainsPod : IWidgetModel
  {
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<FeaturedDomainsPodInfo> PodInfoList { get; set; }
    public FeaturedDomainsPod()
    {
      PodInfoList = new List<FeaturedDomainsPodInfo>();
    }
  }

  public class FeaturedDomainsPodInfo : IWidgetModel
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

