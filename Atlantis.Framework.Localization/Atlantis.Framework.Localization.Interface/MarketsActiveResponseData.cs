using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class MarketsActiveResponseData : IResponseData
  {
    private const string _DEFAULT_MARKET_ID = "en-US";
    private static IMarket _defaultMarket;
    private static MarketsActiveResponseData _defaultResponseData;
    private Dictionary<string, IMarket> _markets; 

    static MarketsActiveResponseData()
    {
      _defaultResponseData = FromCacheDataXml(Properties.Resources.DefaultMarketsActive);

      IMarket defaultMarket;
      _defaultResponseData.TryGetMarketById(_DEFAULT_MARKET_ID, out defaultMarket);
      _defaultMarket = defaultMarket;
    }

    private MarketsActiveResponseData()
    {
      _markets = new Dictionary<string, IMarket>(StringComparer.OrdinalIgnoreCase);
    }

    public static MarketsActiveResponseData DefaultMarkets
    {
      get { return _defaultResponseData; }
    }

    public static IMarket DefaultMarketInfo
    {
      get { return _defaultMarket; }
    }

    public static MarketsActiveResponseData FromCacheDataXml(string cacheDataXml)
    {
      MarketsActiveResponseData responseObject = DefaultMarkets;
      Dictionary<string, IMarket> markets = new Dictionary<string, IMarket>(StringComparer.OrdinalIgnoreCase);

      try
      {
        if (!string.IsNullOrEmpty(cacheDataXml))
        {
          XElement data = XElement.Parse(cacheDataXml);
          foreach (XElement item in data.Descendants("item"))
          {
            XAttribute id = item.Attribute("catalog_marketID");
            XAttribute description = item.Attribute("marketDescription");
            XAttribute msCulture = item.Attribute("MSCulture");
            XAttribute internalOnly = item.Attribute("internalOnly");

            if (id != null && id.Value != String.Empty)
            {
              markets[id.Value] =
                new Market(id.Value, description.Value, msCulture.Value, internalOnly.Value != "0");
            }
          }

          responseObject = new MarketsActiveResponseData(markets);
        }
        else
        {
          var aex = new AtlantisException("Atlantis.Framework.Localization.MarketsActiveResponseData.FromCacheDataXml", 0, "WS Response was empty string.", string.Empty);
          Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("Atlantis.Framework.Localization.MarketsActiveResponseData.FromCacheDataXml", 0, "WS Response contains invalid XML or invalid price values.", message);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
      }

      return responseObject;
    }

    private MarketsActiveResponseData(Dictionary<string, IMarket> markets)
      : this()
    {
      _markets = markets;
    }

    public bool TryGetMarketById(string marketId, out IMarket market)
    {
      return _markets.TryGetValue(marketId, out market);
    }

    public int Count
    {
      get { return _markets.Values.Count; }
    }

    public IEnumerable<IMarket> Markets
    {
      get
      {
        return _markets.Values;
      }
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
