using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class BobBannerPod : IWidgetModel
  {
    public IList<BobBannerInfo> BannerInfoList { get; set; }
    public BobBannerPod()
    {
      BannerInfoList = new List<BobBannerInfo>();
    }
  }

  public class BobBannerInfo : IWidgetModel
  {
    public string Size { get; set; }
    public string BackgroundColor { get; set; }
    public string IconImage { get; set; }
    public string ImagePosition { get; set; }
    public string Title { get; set; }
    public string LinkText { get; set; }
    public string QuoteTitle { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }
}
