using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountrySiteMarketMappingsResponseData : IResponseData
  {
    private static CountrySiteMarketMappingsResponseData _defaultResponseData;
    private Dictionary<string, CountrySiteMarketMapping> _mappingsByCountrySiteAndLanguage;
    private Dictionary<string, CountrySiteMarketMapping> _rawTable;

    static CountrySiteMarketMappingsResponseData()
    {
      _defaultResponseData = new CountrySiteMarketMappingsResponseData();
      _defaultResponseData.NoMappings = true;
    }

    private CountrySiteMarketMappingsResponseData()
    {
      _mappingsByCountrySiteAndLanguage = new Dictionary<string, CountrySiteMarketMapping>(StringComparer.OrdinalIgnoreCase);
      _rawTable = new Dictionary<string, CountrySiteMarketMapping>(StringComparer.OrdinalIgnoreCase);
    }

    public static CountrySiteMarketMappingsResponseData NoMappingsResponse
    {
      get { return _defaultResponseData; }
    }

    public static CountrySiteMarketMappingsResponseData FromCacheDataXml(string cacheDataXml)
    {
      CountrySiteMarketMappingsResponseData responseObject = new CountrySiteMarketMappingsResponseData();

      try
      {
        if (!string.IsNullOrEmpty(cacheDataXml))
        {
          XElement data = XElement.Parse(cacheDataXml);
          foreach (XElement item in data.Descendants("item"))
          {
            XAttribute countrySiteId = item.Attribute("catalog_countrySite");
            XAttribute marketId = item.Attribute("catalog_marketID");
            XAttribute languageUrlSegment = item.Attribute("languageURLSegment");
            XAttribute internalOnly = item.Attribute("internalOnly");

            var csmm = new CountrySiteMarketMapping(countrySiteId.Value, marketId.Value, languageUrlSegment.Value, internalOnly.Value != "0");
            if (languageUrlSegment.Value != String.Empty)
            {
              responseObject._mappingsByCountrySiteAndLanguage[languageUrlSegment.Value] = csmm;
            }

            // add to the representation of the raw table (no need for the countrysite id, since the response object only contains data for the requested countrysite id)
            responseObject._rawTable[marketId.Value] = csmm;
          }
        }
        else
        {
          var aex = new AtlantisException("Atlantis.Framework.Localization.CountrySiteMarketMappingsResponseData.FromCacheDataXml", "0",
                                          "WS Response was empty string.", string.Empty, null, null);
          Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
          responseObject = CountrySiteMarketMappingsResponseData.NoMappingsResponse;
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("Atlantis.Framework.Localization.CountrySiteMarketMappingsResponseData.FromCacheDataXml", "0", "WS Response contains invalid XML or invalid price values.", message, null, null);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
        responseObject = CountrySiteMarketMappingsResponseData.NoMappingsResponse;
      }

      return responseObject;
    }

    public static CountrySiteMarketMappingsResponseData FromCountrySiteAndMarketId(string countrySiteId, string defaultMarketId)
    {
      CountrySiteMarketMappingsResponseData responseObject = new CountrySiteMarketMappingsResponseData();
      responseObject.IsExceptionDefaultMapping = true;
      responseObject._mappingsByCountrySiteAndLanguage[String.Empty] = new CountrySiteMarketMapping(countrySiteId, defaultMarketId, String.Empty, false);
      return responseObject;
    }

    public bool IsValidUrlLanguageForCountrySite(string language, bool includeInternalOnly)
    {
      bool result = false;

      CountrySiteMarketMapping mapping;
      if (TryGetMapping(language, out mapping))
      {
        result = (!mapping.IsInternalOnly || includeInternalOnly);
      }

      return result;
    }

    public bool TryGetMarketIdByCountrySiteAndUrlLanguage(string language, out string marketId)
    {
      marketId = string.Empty;

      CountrySiteMarketMapping mapping;

      if (TryGetMapping(language, out mapping))
      {
        marketId = mapping.MarketId;
        return true;
      }
      
      return false;
    }

    private bool TryGetMapping(string language, out CountrySiteMarketMapping mapping)
    {
      return _mappingsByCountrySiteAndLanguage.TryGetValue((IsExceptionDefaultMapping ? String.Empty : language),
                                                           out mapping);
    }

    public string GetLanguageUrl(string marketId)
    {
      CountrySiteMarketMapping csmm;
      _rawTable.TryGetValue(marketId, out csmm);
      return csmm != null ? csmm.LanguageUrlSegment : String.Empty;
    }
	
    public bool IsPublicMapping(string marketId)
    {
      CountrySiteMarketMapping csmm;
      _rawTable.TryGetValue(marketId, out csmm);
      return csmm != null && !csmm.IsInternalOnly;

    }

    public bool NoMappings { get; private set; }

    private bool IsExceptionDefaultMapping { get; set; }

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
