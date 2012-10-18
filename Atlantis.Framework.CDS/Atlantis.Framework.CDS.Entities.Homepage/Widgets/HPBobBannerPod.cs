using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPBobBannerPod : IWidgetModel
  {
    public IList<HPBobBannerInfo> BannerInfoList { get; set; }
    public HPBobBannerPod()
    {
      BannerInfoList = new List<HPBobBannerInfo>();
    }
  }

  public class HPBobBannerInfo : IWidgetModel
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
