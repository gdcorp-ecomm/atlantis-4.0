using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPTechPanel : IWidgetModel
  {
    public IList<HPInsideInfo> InsideInfoList { get; set; }
    public string BackgroundImage { get; set; }
    public string PanelTitle { get; set; }
    public string BelowSearchTeaser { get; set; }
    public string PanelSwitchLinkTitle { get; set; }
    public string PanelSwitchLinkPanel { get; set; }
    public string DomainSearchTitle { get; set; }
    public IList<HPTechMarketingAd> MarketingAdList { get; set; }
    public IList<HPTechSnipeInfo> SnipeInfoList { get; set; }
    public HPTechSocialMediaData SocialMediaData { get; set; }
    public HPTechPanel()
    {
      InsideInfoList = new List<HPInsideInfo>();
      MarketingAdList = new List<HPTechMarketingAd>();
      SnipeInfoList = new List<HPTechSnipeInfo>();
      SocialMediaData = new HPTechSocialMediaData();
    }
  }

  public class HPInsideInfo : IWidgetModel
  {
    public IList<HPInsideInfoItem> InsideInfoItemList { get; set; }
    public string InsideImage { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public HPInsideInfo()
    {
      InsideInfoItemList = new List<HPInsideInfoItem>();
    }
  }

  public class HPInsideInfoItem : IWidgetModel
  {
    public string ImagePosition { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string LinkName { get; set; }
  }

  public class HPTechSnipeInfo : IWidgetModel
  {
    public string SnipeText { get; set; }
    public string SnipeHoverText { get; set; }
    public string CountryCodesList { get; set; }
  }

  public class HPTechMarketingAd : IWidgetModel
  {
    public string AdText { get; set; }
    public string AdLink { get; set; }
    public string AdLinkText { get; set; }
    public string HoverText { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }

  public class HPTechSocialMediaData : IWidgetModel
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
