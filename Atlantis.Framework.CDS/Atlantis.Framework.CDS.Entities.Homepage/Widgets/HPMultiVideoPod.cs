using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPMultiVideoPod : IWidgetModel
  {
    public IList<HPVideoPodItem> VideoPodItemList { get; set; }
    public HPMultiVideoPod()
    {
      VideoPodItemList = new List<HPVideoPodItem>();
    }
  }

  public class HPVideoPodItem : IWidgetModel
  {
    public string PodTitle { get; set; }
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<HPVideo> VideosList { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public HPVideoPodItem()
    {
      VideosList = new List<HPVideo>();
    }
  }

  public class HPVideo : IWidgetModel
  {
    public string IconImage { get; set; }
    public string ImagePositionTop { get; set; }
    public string ImagePositionLeft { get; set; }
    public string VideoTitle { get; set; }
    public string VideoMediaId { get; set; }
    public string VideoLanguage { get; set; }
    public string VideoPopInLink { get; set; }
  }
}
