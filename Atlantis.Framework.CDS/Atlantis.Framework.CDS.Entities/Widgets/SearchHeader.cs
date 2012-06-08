using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SearchHeader : IWidgetModel
  {
    public SearchHeader()
    {
      SocialData = new SocialMediaData();
      OptionalBanner = new Banner();
    }

    public string Title { get; set; }
    public string Tld { get; set; }
    public string BgImage { get; set; }
    public string HeaderText { get; set; }
    public string SearchCi { get; set; }
    public string BulkCi { get; set; }
    public string SubheaderText { get; set; }
    public SocialMediaData SocialData { get; set; }
    public Banner OptionalBanner { get; set; }

    public class SocialMediaData
    {
      public string CanonicalUrl { get; set; }
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

    public class Banner
    {
      public bool DisplayBanner { get; set; }
      public string BannerCustomStyle { get; set; }
      public string OnClick { get; set; }
      public string ImageUrl { get; set; }
      public int Width { get; set; }
      public int Height { get; set; }
      public string Text { get; set; }
      public int Top { get; set; }
      public int Right { get; set; }
      public string Float { get; set; }
    }
  }
}
