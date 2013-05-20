using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class RegionResponseData : IResponseData
  {
    public static RegionResponseData Empty { get; private set; }

    static RegionResponseData()
    {
      Empty = new RegionResponseData(new HashSet<int>());
    }

    private AtlantisException _exception = null;
    private HashSet<int> _countryIds;

    public static RegionResponseData FromDataCacheXml(string regionsXml)
    {
      XElement dataElement = XElement.Parse(regionsXml);

      HashSet<int> countryIds = new HashSet<int>();
      var itemElements = dataElement.Descendants("item");
      foreach (var itemElement in itemElements)
      {
        XAttribute countryIdAtt = itemElement.Attribute("CountryId");
        if (countryIdAtt != null)
        {
          countryIds.Add(Convert.ToInt32(countryIdAtt.Value));
        }
      }

      if (countryIds.Count > 0)
      {
        return new RegionResponseData(countryIds);
      }
      else
      {
        return Empty;
      }

    }

    public static RegionResponseData FromException(AtlantisException exception)
    {
      return new RegionResponseData(exception);
    }

    private RegionResponseData(HashSet<int> countryIds)
    {
      _countryIds = countryIds;
    }

    private RegionResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public IEnumerable<int> CountryIds
    {
      get { return _countryIds; }
    }

    public bool HasCountry(int countryId)
    {
      return _countryIds.Contains(countryId);
    }

    public int Count
    {
      get 
      {
        if (_countryIds != null)
        {
          return _countryIds.Count;
        }
        else
        {
          return 0;
        }
      }
    }

    public string ToXML()
    {
      XElement region = new XElement("region");

      if (_countryIds != null)
      {
        foreach (int countryId in _countryIds)
        {
          XElement countryElement = new XElement("country");
          countryElement.Add(new XAttribute("id", countryId.ToString()));
          region.Add(countryElement);
        }
      }

      return region.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
