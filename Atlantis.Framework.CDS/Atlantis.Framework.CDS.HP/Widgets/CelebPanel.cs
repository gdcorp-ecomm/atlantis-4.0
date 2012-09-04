using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.HP.Widgets
{
  public class CelebPanel
  {
    public IList<CelebInfo> CelebInfoList { get; set; }
    public string PanelTitle { get; set; }
    public string BelowSearchTeaser { get; set; }
    public string PanelSwitchLinkTitle { get; set; }
    public string PanelSwitchLinkPanel { get; set; }
    public string DomainSearchTitle { get; set; }
    public string MarketingAd { get; set; }
    public IList<SnipeInfo> SnipeInfoList { get; set; }

    public CelebPanel()
    {
      CelebInfoList = new List<CelebInfo>();
      SnipeInfoList = new List<SnipeInfo>();
    }
  }

  public class CelebInfo
  {
    public string BackgroundImage { get; set; }
    public string CelebName { get; set; }
    public string CelebSignature { get; set; }
    public string SignaturePositionTop { get; set; }
    public string SignaturePositionRight { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitAppSettingName { get; set; }
  }

  public class SnipeInfo
  {
    public string SnipeText { get; set; }
    public string SnipeHoverText { get; set; }
    public string CountryCodesList { get; set; }
  }
}
