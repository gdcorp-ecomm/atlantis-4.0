using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPBannerPod : IWidgetModel
  {
    public IList<HPBannerInfo> BannerInfoList { get; set; }

    public HPBannerPod()
    {
      BannerInfoList = new List<HPBannerInfo>();
    }
  }

  public class HPBannerInfo : IWidgetModel
  {
    public string Size { get; set; }
    public string BackgroundColor { get; set; }
    public string IconImage { get; set; }
    public string ImagePosition { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string TextAlign { get; set; }
    public string TextSize { get; set; }
    public string Link { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
