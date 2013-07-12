using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class IPLocationLookupResponseData : IResponseData
  {
    public static IPLocationLookupResponseData NotFound { get; private set; }

    static IPLocationLookupResponseData()
    {
      NotFound = new IPLocationLookupResponseData(IPLocation.Unknown);
    }

    public static IPLocationLookupResponseData FromIPLocation(IPLocation location)
    {
      if ((location == null) || (location == IPLocation.Unknown))
      {
        return NotFound;
      }
      else
      {
        return new IPLocationLookupResponseData(location);
      }
    }

    private IPLocation _location;

    private IPLocationLookupResponseData(IPLocation location)
    {
      _location = location;
    }

    public IPLocation Location
    {
      get { return _location; }
    }

    public bool LocationFound
    {
      get { return _location != IPLocation.Unknown; }
    }

    public string ToXML()
    {
      XElement element = new XElement("IPLocationLookupResponseData");
      element.Add(
        new XAttribute("found", LocationFound.ToString()),
        new XAttribute("countrycode", Location.CountryCode),
        new XAttribute("city", Location.City),
        new XAttribute("latitude", Location.Latitude),
        new XAttribute("longitude", Location.Longitude),
        new XAttribute("metrocode", Location.MetroCode),
        new XAttribute("postalcode", Location.PostalCode),
        new XAttribute("region", Location.Region),
        new XAttribute("regionname", Location.RegionName));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
