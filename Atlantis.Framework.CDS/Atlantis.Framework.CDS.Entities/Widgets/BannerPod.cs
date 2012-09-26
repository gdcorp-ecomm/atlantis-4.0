using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class BannerPod : IWidgetModel
  {
    public IList<BannerInfo> BannerInfoList { get; set; }

    public BannerPod()
    {
      BannerInfoList = new List<BannerInfo>();
    }
  }

  public class BannerInfo : IWidgetModel
  {
    public string Size { get; set; }
    public string BackgroundColor { get; set; }
    public string IconImage { get; set; }
    public string ImagePosition { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string TextAlign { get; set; }
    public string TextSize { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
