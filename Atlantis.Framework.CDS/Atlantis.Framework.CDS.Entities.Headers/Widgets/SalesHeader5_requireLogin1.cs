using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.Headers.Widgets
{
  public class SalesHeader5_requireLogin1 : IWidgetModel, IContent, IAddToCartWidgetModel
  {
    public string HeaderImageFile { get; set; }
    public string HeaderSpriteFile { get; set; }
    public string MainHeading { get; set; }
    public int SubHeadingWidth { get; set; }
    public bool MainHeadingOn2Lines { get; set; }
    public int RightListLeft { get; set; }
    public int BottomRightTextTop { get; set; }
    public int BottomRightTextLeft { get; set; }
    public bool Show99PercentBanner { get; set; }
    public bool HasMobileCorner { get; set; }
    public List<SubHeadingItem> SubHeadings { get; set; }
    public List<BottomRightText> CurrentBottomRightText { get; set; }
    public List<RightListItem> CurrentRightList { get; set; }
    public bool HideCloudIcon { get; set; }
    public bool HasSupportBanner { get; set; }
    public string SupportBannerText { get; set; }
    public bool Filtered { get; set; }
    public string Custom99PercentContent { get; set; }
    public int SubHeadTop { get; set; }
    public List<string> ContentList { get; set; }
    public string OverrideStyles { get; set; }

    public string CiCode { get; set; }
    public string CrossSellConfig { get; set; }
    public string FormName { get; set; }
    public string FormActionUrl { get; set; }
    public bool FromDPP { get; set; }
    public string ISC { get; set; }
    public string ItemTrackingCode { get; set; }
    public bool RedirectStraightToCart { get; set; }
    public bool SkipDomainerCheck { get; set; }
    public string Upsell { get; set; }
    public bool UseNewCrossSellPage { get; set; }
    public string XS_AppSettingKey { get; set; }
    public string XS_ContainerCode { get; set; }
    public string XS_LaunchCICode { get; set; }

    [Obsolete("Included for backwards compatibility only. Use \"Buttons\" property instead.")]
    public List<SalesHeader2.MarketingButton> ButtonList { get; set; }

    private HeadersSocialMedia _socialData;
    public HeadersSocialMedia SocialData
    {
      get
      {
        if (_socialData == null)
        {
          _socialData = new HeadersSocialMedia();
        }
        return _socialData;
      }
      set { _socialData = value; }
    }

    private int _headerImageWidth;
    public int HeaderImageWidth
    {
      get
      {
        if (_headerImageWidth == default(int))
        {
          _headerImageWidth = 425;
        }
        return _headerImageWidth;
      }
      set { _headerImageWidth = value; }
    }

    private int _headerImageHeight;
    public int HeaderImageHeight
    {
      get
      {
        if (_headerImageHeight == default(int))
        {
          _headerImageHeight = 200;
        }
        return _headerImageHeight;
      }
      set { _headerImageHeight = value; }
    }

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

    private int? _headerImageTopSpacing;
    public int HeaderImageTopSpacing
    {
      get
      {
        if (!_headerImageTopSpacing.HasValue)
        {
          _headerImageTopSpacing = 10;
        }
        return _headerImageTopSpacing.Value;
      }
      set { _headerImageTopSpacing = value; }
    }

    private int? _headerImageRightSpacing;
    public int HeaderImageRightSpacing
    {
      get
      {
        if (!_headerImageRightSpacing.HasValue)
        {
          _headerImageRightSpacing = 10;
        }
        return _headerImageRightSpacing.Value;
      }
      set { _headerImageRightSpacing = value; }
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

    private List<ProductOption> _productOptions;
    public List<ProductOption> ProductOptions
    {
      get
      {
        if (_productOptions == null)
        {
          _productOptions = new List<ProductOption>();
        }
        return _productOptions;
      }
      set
      {
        _productOptions = value;
      }
    }

    private List<HeadersMarketingButton2> _buttons;
    public List<HeadersMarketingButton2> Buttons
    {
      get
      {
        if (_buttons == null)
        {
          _buttons = new List<HeadersMarketingButton2>();
        }
        return _buttons;
      }
      set
      {
        _buttons = value;
      }
    }

    private string _mainHeadingTag;
    public string MainHeadingTag
    {
      get
      {
        if (string.IsNullOrEmpty(_mainHeadingTag))
        {
          _mainHeadingTag = "h1";
        }
        return _mainHeadingTag;
      }
      set
      {
        _mainHeadingTag = value;
      }
    }
  }
}
