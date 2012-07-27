using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public enum CDSPixelTypes
  {
    YahooDomains,
    YahooHosting,
    YahooSSL,
    GoogleAdwords,
    GoogleAdwordsSearchPage,
    GoogleAdwordsHomePage,
    SSLGoogleAdwords,
    GoogleAdwordsResellerPage,
    GoogleAdwordsWwdPage,
    GoogleAdwordsInclude,
    Bizo,
    Verisign
  }

  public class TrackingPixel : IWidgetModel
  {
    public List<string> PixelList { get; set; }

    private string _pixelType;
    public string PixelType
    {
      get
      {
        return _pixelType;
      }
      set
      {
        CDSPixelTypes pixelTypes;
        if (Enum.TryParse<CDSPixelTypes>(value, out pixelTypes))
        {
          _pixelType = value;
        }
        else
        {
          throw new ArgumentException("Invalid pixel type");
        }
      }
    }
  }
}
