using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class CelebPanel : IWidgetModel
  {
    public IList<CelebInfo> CelebInfoList { get; set; }
    public string PanelTitle { get; set; }
    public string BelowSearchTeaser { get; set; }
    public string PanelSwitchLinkTitle { get; set; }
    public string PanelSwitchLinkPanel { get; set; }
    public string DomainSearchTitle { get; set; }
    public IList<MarketingAd> MarketingAdList { get; set; }
    public IList<SnipeInfo> SnipeInfoList { get; set; }
    public CelebPanel()
    {
      CelebInfoList = new List<CelebInfo>();
      MarketingAdList = new List<MarketingAd>();
      SnipeInfoList = new List<SnipeInfo>();
    }
  }

  public class CelebInfo : IWidgetModel
  {
    public string BackgroundImage { get; set; }
    public string BackgroundHeight { get; set; }
    public string CelebName { get; set; }
    public string CelebSignature { get; set; }
    public string SignaturePositionTop { get; set; }
    public string SignaturePositionRight { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }

  public class SnipeInfo : IWidgetModel
  {
    public string SnipeText { get; set; }
    public string SnipeHoverText { get; set; }
    public string CountryCodesList { get; set; }
  }

  public class MarketingAd : IWidgetModel
  {
    public string AdText { get; set; }
    public string AdLink { get; set; }
    public string AdLinkText { get; set; }
    public string HoverText { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }

}
