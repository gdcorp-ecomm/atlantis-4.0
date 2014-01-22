using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountrySitesActiveResponseData : IResponseData
  {
    //    Request Format
    //    <CountrySiteGetActive/>

    //    Response Format
    //    <data count="2">
    //      <item catalog_countrySite="au" catalog_priceGroupID="0" countrySiteDescription="Australia" isActive="-1" internalOnly="0" defaultMarketID="en-AU" defaultCurrencyType="AUD"/>
    //      <item catalog_countrySite="ca" catalog_priceGroupID="0" countrySiteDescription="Canada" isActive="-1" internalOnly="0" defaultMarketID="en-CA" defaultCurrencyType="CAD"/>
    //    </data>

    private const string _DEFAULT_COUNTRYSITE_ID = "WWW";
    private static ICountrySite _defaultCountrySite;
    private static CountrySitesActiveResponseData _defaultResponseData;
    private Dictionary<string, ICountrySite> _countrySites; 

    static CountrySitesActiveResponseData()
    {
      _defaultResponseData = FromCacheDataXml(Properties.Resources.DefaultCountrySitesActive);

      ICountrySite defaultCountrySite;
      _defaultResponseData.TryGetCountrySiteById(_DEFAULT_COUNTRYSITE_ID, out defaultCountrySite);
      _defaultCountrySite = defaultCountrySite;
    }

    private CountrySitesActiveResponseData()
    {
      _countrySites = new Dictionary<string, ICountrySite>(StringComparer.OrdinalIgnoreCase);
    }

    public static CountrySitesActiveResponseData DefaultCountrySites
    {
      get { return _defaultResponseData; }
    }

    public static ICountrySite DefaultCountrySiteInfo
    {
      get { return _defaultCountrySite; }
    }

    public static CountrySitesActiveResponseData FromCacheDataXml(string cacheDataXml)
    {
      CountrySitesActiveResponseData responseObject = DefaultCountrySites;
      Dictionary<string, ICountrySite> countrySites = new Dictionary<string, ICountrySite>(StringComparer.OrdinalIgnoreCase);

      try
      {
        if (!string.IsNullOrEmpty(cacheDataXml))
        {
          XElement data = XElement.Parse(cacheDataXml);
          foreach (XElement item in data.Descendants("item"))
          {
            XAttribute id = item.Attribute("catalog_countrySite");
            XAttribute description = item.Attribute("countrySiteDescription");
            XAttribute priceGroupId = item.Attribute("catalog_priceGroupID");
            XAttribute internalOnly = item.Attribute("internalOnly");
            XAttribute defaultCurrencyType = item.Attribute("defaultCurrencyType");
            XAttribute defaultMarketId = item.Attribute("defaultMarketID");

            if (id != null && id.Value != String.Empty)
            {
              countrySites[id.Value] =
                  new CountrySite(id.Value, description.Value, int.Parse(priceGroupId.Value),
                                  internalOnly.Value != "0",
                                  defaultCurrencyType.Value, 
                                  defaultMarketId.Value);
            }
          }

          responseObject = new CountrySitesActiveResponseData(countrySites);
        }
        else
        {
          var aex = new AtlantisException("Atlantis.Framework.Localization.CountrySitesActiveResponseData.FromCacheDataXml", 0, "WS Response was empty string.", string.Empty);
          Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("Atlantis.Framework.Localization.CountrySitesActiveResponseData.FromCacheDataXml", 0, "WS Response contains invalid XML or invalid price values.", message);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
      }

      return responseObject;
    }

    private CountrySitesActiveResponseData(Dictionary<string, ICountrySite> countrySites) : this()
    {
      _countrySites = countrySites;
    }

    public bool IsValidCountrySite(string countrySiteId, bool includeInternalOnly)
    {
      ICountrySite countrySite;
      if (TryGetCountrySiteById(countrySiteId, out countrySite) && (!countrySite.IsInternalOnly || includeInternalOnly))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool TryGetCountrySiteById(string countrySiteId, out ICountrySite countrySite)
    {
      return _countrySites.TryGetValue(countrySiteId, out countrySite);
    }

    public int Count
    {
      get { return _countrySites.Values.Count; }
    }

    public IEnumerable<ICountrySite> CountrySites
    {
      get
      {
        return _countrySites.Values;
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
