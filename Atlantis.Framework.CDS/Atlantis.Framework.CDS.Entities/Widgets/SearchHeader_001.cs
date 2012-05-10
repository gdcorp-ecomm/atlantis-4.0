using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SearchHeaders_001 : IWidgetModel
  {
    public string Title { get; set; }
    public string Tld { get; set; }
    public string BgImage { get; set; }
    public string HeaderText { get; set; }
    public string SearchCi { get; set; }
    public string BulkCi { get; set; }
    public string SubheaderText { get; set; }    
    public Banner OptionalBanner { get; set; }    

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
