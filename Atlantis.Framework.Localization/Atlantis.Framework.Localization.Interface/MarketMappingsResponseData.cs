using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class MarketMappingsResponseData : ResponseData
  {
    private Dictionary<string, CountrySiteMarketMapping> _mappings;

    static MarketMappingsResponseData()
    {
      NoMappingsResponse = new MarketMappingsResponseData();
      NoMappingsResponse.NoMappings = true;
    }

    private MarketMappingsResponseData()
    {
      _mappings = new Dictionary<string, CountrySiteMarketMapping>(StringComparer.OrdinalIgnoreCase);
    }

    public static MarketMappingsResponseData NoMappingsResponse { get; private set; }

    public static MarketMappingsResponseData FromCacheDataXml(string cacheDataXml)
    {
      MarketMappingsResponseData responseObject = new MarketMappingsResponseData();

      XElement data = XElement.Parse(cacheDataXml);
      foreach (XElement item in data.Descendants("item"))
      {
        XAttribute countrySiteId = item.Attribute("catalog_countrySite");
        XAttribute marketId = item.Attribute("catalog_marketID");
        XAttribute languageUrlSegment = item.Attribute("languageURLSegment");
        XAttribute internalOnly = item.Attribute("internalOnly");

        var csmm = new CountrySiteMarketMapping(countrySiteId.Value, marketId.Value, languageUrlSegment.Value, internalOnly.Value != "0");
        if (countrySiteId.Value != String.Empty)
        {
          responseObject._mappings[countrySiteId.Value] = csmm;
        }
      }

      return responseObject;
    }

    public string GetFirstMappedCountrySiteId(bool includeInternalOnly)
    {
      CountrySiteMarketMapping mapping = null;
      if (includeInternalOnly)
      {
        mapping = _mappings.Values.FirstOrDefault();
      }
      else
      {
        mapping = _mappings.Values.FirstOrDefault(m => !m.IsInternalOnly);
      }
      return mapping == null ? string.Empty : mapping.CountrySiteId;
    }

    public string GetUrlLanguageForCountrySite(string countrySiteId, bool includeInternalOnly)
    {
      CountrySiteMarketMapping mapping;
      if (_mappings.TryGetValue(countrySiteId, out mapping))
      {
        return (!mapping.IsInternalOnly || includeInternalOnly) ? mapping.LanguageUrlSegment : string.Empty;
      }

      return string.Empty;
    }

    public bool IsPublicMapping(string countrySiteId)
    {
      return IsMappedCountrySite(countrySiteId, false);
    }

    public bool IsMappedCountrySite(string countrySiteId, bool bIncludeInternal)
    {
      CountrySiteMarketMapping csmm;
      _mappings.TryGetValue(countrySiteId, out csmm);
      return csmm != null && (!csmm.IsInternalOnly || bIncludeInternal);
    }

    public bool NoMappings { get; private set; }

  }
}
