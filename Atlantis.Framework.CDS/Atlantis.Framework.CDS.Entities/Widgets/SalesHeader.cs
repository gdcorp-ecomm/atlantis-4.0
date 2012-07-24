using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SalesHeader : IWidgetModel
  {
    public SalesHeader()
    {
    }

    public string HeaderImageFile { get; set; }
    public string MainHeading { get; set; }

    private int _mainHeadingWidth;
    public int MainHeadingWidth
    {
      get
      {
        if (_mainHeadingWidth == default(int))
        {
          _mainHeadingWidth = 416;
        }
        return _mainHeadingWidth;
      }
      set
      {
        _mainHeadingWidth = value;
      }
    }

    public int SubHeadingWidth { get; set; }
    public bool MainHeadingOn2Lines { get; set; }
    public int RightListLeft { get; set; }

    private int _rightListTop;
    public int RightListTop
    {
      get
      {
        if (_rightListTop == default(int))
        {
          _rightListTop = 52;
        }
        return _rightListTop;
      }
      set
      {
        _rightListTop = value;
      }
    }

    public int BottomRightTextTop { get; set; }
    public int BottomRightTextLeft { get; set; }
    public bool Show99PercentBanner { get; set; }
    public bool HasMobileCorner { get; set; }
    public List<SubHeadingItem> SubHeadings { get; set; }
    public List<MarketingButton> ButtonList { get; set; }
    public List<BottomRightText> CurrentBottomRightText { get; set; }
    public List<RightListItem> CurrentRightList { get; set; }
    public bool HideCloudIcon { get; set; }
    public bool HasSupportBanner { get; set; }
    public string SupportBannerText { get; set; }
    public bool Filtered { get; set; }
    public int MainHeaderHeight { get; set; }

    private SocialMediaData _socialData;
    public SocialMediaData SocialData
    {
      get 
      {
        if (_socialData == null)
        {
          _socialData = new SocialMediaData();
        }
        return _socialData; 
      }
      set { _socialData = value; }
    }

    public class MarketingButton
    {
      public string LargeText { get; set; }
      public string SmallText { get; set; }
      public bool IsMainButton { get; set; }
    }

    public class BottomRightText : ElementBase
    {
      public bool IsWhiteText { get; set; }
    }

    public class RightListItem : ElementBase
    {
      public bool IsGoldText { get; set; }
    }

    public class SubHeadingItem : ElementBase
    {
      public SubHeadingItem()
      {
        this.Text = string.Empty;
      }
    }

    public class SocialMediaData
    {
      public string FacebookUrl { get; set; }
      public string Description { get; set; }
      public string ImageUrl { get; set; }
      public string ItemType { get; set; }
      public string Title { get; set; }
      public string TweetText { get; set; }
      public string TweetUrl { get; set; }
      public string TweetRelated { get; set; }
      public bool UseFacebook { get; set; }
      public bool UseTwitter { get; set; }
      public bool UseGooglePlus { get; set; }
    }
  }
}
