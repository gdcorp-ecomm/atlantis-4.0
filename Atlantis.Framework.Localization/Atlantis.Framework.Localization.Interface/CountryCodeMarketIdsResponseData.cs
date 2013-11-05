using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountryCodeMarketIdsResponseData : IResponseData
  {
    #region Properties
    public IEnumerable<string> MarketIds { get; private set; }
    public bool HasMarketIds { get; private set; }
    public static CountryCodeMarketIdsResponseData NoMarketsResponse { get; private set; }

    #endregion

    static CountryCodeMarketIdsResponseData()
    {
      NoMarketsResponse = new CountryCodeMarketIdsResponseData(new List<string>(), false);
    }

    private CountryCodeMarketIdsResponseData(IEnumerable<string> marketIds, bool hasMarketIds )
    { 
      MarketIds = marketIds;
      HasMarketIds = hasMarketIds;
    }

    public static CountryCodeMarketIdsResponseData FromCacheDataXml(string cacheDataXml)
    {
      CountryCodeMarketIdsResponseData response;
      var marketIds = new List<string>(5);

      try
      {
        if (!string.IsNullOrEmpty(cacheDataXml))
        {
          var data = XElement.Parse(cacheDataXml);
          foreach (var item in data.Descendants("item"))
          {
            marketIds.Add(item.Attribute("catalog_marketID").Value);
          }

          response = marketIds.Count > 0 ? new CountryCodeMarketIdsResponseData(marketIds, true) : NoMarketsResponse;
        }
        else
        {
          var aex = new AtlantisException("Atlantis.Framework.Localization.CountryCodeMarketIdsResponseData.FromCacheDataXml", "0", "WS Response was empty string.", string.Empty, null, null);
          Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
          response = NoMarketsResponse;
        }
      }
      catch (Exception ex)
      {
        var message = string.Concat(ex.Message, Environment.NewLine, ex.StackTrace);
        var aex = new AtlantisException("Atlantis.Framework.Localization.CountryCodeMarketIdsResponseData.FromCacheDataXml", "0", "WS Response contains invalid XML", message, null, null);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
        response = NoMarketsResponse;
      }

      return response;

    }

    #region IResponseData Members
    public string ToXML()
    {
      return String.Empty;
    }

    public AtlantisException GetException()
    {
      return null;
    }
    #endregion
  }
}
