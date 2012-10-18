using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPCelebPanel : IWidgetModel
  {
    public IList<HPCelebInfo> CelebInfoList { get; set; }
    public string PanelTitle { get; set; }
    public string BelowSearchTeaser { get; set; }
    public string PanelSwitchLinkTitle { get; set; }
    public string PanelSwitchLinkPanel { get; set; }
    public string DomainSearchTitle { get; set; }
    public IList<HPMarketingAd> MarketingAdList { get; set; }
    public IList<HPSnipeInfo> SnipeInfoList { get; set; }
    public HPSocialMediaData SocialMediaData { get; set; }
    public HPCelebPanel()
    {
      CelebInfoList = new List<HPCelebInfo>();
      MarketingAdList = new List<HPMarketingAd>();
      SnipeInfoList = new List<HPSnipeInfo>();
      SocialMediaData = new HPSocialMediaData();
    }
  }

  public class HPCelebInfo : IWidgetModel
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

  public class HPSnipeInfo : IWidgetModel
  {
    public string SnipeText { get; set; }
    public string SnipeHoverText { get; set; }
    public string CountryCodesList { get; set; }
  }

  public class HPMarketingAd : IWidgetModel
  {
    public string AdText { get; set; }
    public string AdLink { get; set; }
    public string AdLinkText { get; set; }
    public string HoverText { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }

  public class HPSocialMediaData
  {
    public string FacebookUrl { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string ItemType { get; set; }
    public string Title { get; set; }
    public string TweetText { get; set; }
    public string TweetUrl { get; set; }
    public string TweetRelated { get; set; }
    public string TweetHash { get; set; }
    public bool UseFacebook { get; set; }
    public bool UseTwitter { get; set; }
    public bool UseGooglePlus { get; set; }
  }

}
