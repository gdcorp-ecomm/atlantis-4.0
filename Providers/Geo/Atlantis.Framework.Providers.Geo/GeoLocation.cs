using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoLocation : IGeoLocation
  {
    internal static GeoLocation FromNotFound()
    {
      return new GeoLocation(IPLocationLookupResponseData.NotFound.Location);
    }

    internal static GeoLocation FromIPLocation(IPLocation location)
    {
      if (location == null)
      {
        location = IPLocationLookupResponseData.NotFound.Location;
      }

      return new GeoLocation(location);
    }

    private readonly IPLocation _ipLocation;

    private GeoLocation(IPLocation location)
    {
      _ipLocation = location;
    }

    public string CountryCode
    {
      get { return _ipLocation.CountryCode; }
    }

    public string GeoRegion
    {
      get { return _ipLocation.Region; }
    }

    public string GeoRegionName
    {
      get { return _ipLocation.RegionName; }
    }

    public string City
    {
      get { return _ipLocation.City; }
    }

    public string PostalCode
    {
      get { return _ipLocation.PostalCode; }
    }

    public double Latitude
    {
      get { return _ipLocation.Latitude; }
    }

    public double Longitude
    {
      get { return _ipLocation.Longitude; }
    }

    public int MetroCode
    {
      get { return _ipLocation.MetroCode; }
    }
  }
}
