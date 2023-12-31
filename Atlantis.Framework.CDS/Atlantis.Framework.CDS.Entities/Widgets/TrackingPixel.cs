﻿using System;
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
    Verisign,
    YahooRemarketingHP,
    GoogleRemarketingHP,
    AdvertisingHP,
    ValueClickRetargetingHP,
    ValueClickRetargetingWSB,
    ValueClickRetargetingDLP,
    GoogleIndiaDomains,
    GoogleIndiaHomePage,
    GoogleIndiaHosting,
    GoogleIndiaSSL,
    AdvertisingWSB,
    GoogleAuDomains,
    GoogleAuHomePage,
    GoogleAuHosting,
    GoogleAuSSL,
    GoogleCaDomains,
    GoogleCaHomePage,
    GoogleCaHosting,
    GoogleCaSSL,
    GoogleEsDomains,
    GoogleEsHomePage,
    GoogleEsHosting,
    GoogleEsSSL,
    FacebookRetargetingDPP,
    FacebookRetargetingWH,
    FacebookRetargetingWSB,
    RocketFuelUS
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
