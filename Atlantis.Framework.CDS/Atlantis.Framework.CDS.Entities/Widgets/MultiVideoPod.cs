using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class MultiVideoPod : IWidgetModel
  {
    public IList<VideoPodItem> VideoPodItemList { get; set; }
    public MultiVideoPod()
    {
      VideoPodItemList = new List<VideoPodItem>();
    }
  }

  public class VideoPodItem : IWidgetModel
  {
    public string PodTitle { get; set; }
    public string BackgroundColor { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string BorderColor { get; set; }
    public string HoverBorderColor { get; set; }
    public IList<Video> VideosList { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public VideoPodItem()
    {
      VideosList = new List<Video>();
    }
  }

  public class Video : IWidgetModel
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
