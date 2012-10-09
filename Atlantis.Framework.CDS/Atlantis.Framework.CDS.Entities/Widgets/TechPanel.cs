using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class TechPanel : IWidgetModel
  {
    public IList<InsideInfo> InsideInfoList { get; set; }
    public string BackgroundImage { get; set; }
    public string PanelTitle { get; set; }
    public string BelowSearchTeaser { get; set; }
    public string PanelSwitchLinkTitle { get; set; }
    public string PanelSwitchLinkPanel { get; set; }
    public string DomainSearchTitle { get; set; }
    public IList<TechMarketingAd> MarketingAdList { get; set; }
    public IList<TechSnipeInfo> SnipeInfoList { get; set; }
    public TechSocialMediaData SocialMediaData { get; set; }
    public TechPanel()
    {
      InsideInfoList = new List<InsideInfo>();
      MarketingAdList = new List<TechMarketingAd>();
      SnipeInfoList = new List<TechSnipeInfo>();
      SocialMediaData = new TechSocialMediaData();
    }
  }

  public class InsideInfo : IWidgetModel
  {
    public IList<InsideInfoItem> InsideInfoItemList { get; set; }
    public string InsideImage { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
    public InsideInfo()
    {
      InsideInfoItemList = new List<InsideInfoItem>();
    }
  }

  public class InsideInfoItem : IWidgetModel
  {
    public string ImagePosition { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string LinkName { get; set; }
  }

  public class TechSnipeInfo : IWidgetModel
  {
    public string SnipeText { get; set; }
    public string SnipeHoverText { get; set; }
    public string CountryCodesList { get; set; }
  }

  public class TechMarketingAd : IWidgetModel
  {
    public string AdText { get; set; }
    public string AdLink { get; set; }
    public string AdLinkText { get; set; }
    public string HoverText { get; set; }
    public string CountryCodesList { get; set; }
    public string SplitValue { get; set; }
  }

  public class TechSocialMediaData : IWidgetModel
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
