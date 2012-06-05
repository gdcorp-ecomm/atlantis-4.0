using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SalesHeader2 : IWidgetModel
  {
    public SalesHeader2()
    {
      MainHeadingWidth = 416;
      RightListTop = 52;
      HasSupportBanner = false;
    }

    public string HeaderImageFile { get; set; }
    public string HeaderSpriteFile { get; set; }
    public string MainHeading { get; set; }
    public int MainHeadingWidth { get; set; }
    public int SubHeadingWidth { get; set; }
    public bool MainHeadingOn2Lines { get; set; }
    public int RightListLeft { get; set; }
    public int RightListTop { get; set; }
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
  }
}
