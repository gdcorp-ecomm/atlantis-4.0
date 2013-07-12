namespace Atlantis.Framework.Providers.Geo.Interface
{
  /// <summary>
  /// Detailed location data from the GeoIPLocation.dat (city file)
  /// </summary>
  public interface IGeoLocation
  {
    /// <summary>
    /// 2 letter country code
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Short region name if available.  This is NOT related to our datacache regions
    /// </summary>
    string GeoRegion { get; }

    /// <summary>
    /// Region name if available.  This is NOT related to our datacache regions
    /// </summary>
    string GeoRegionName { get; }

    /// <summary>
    /// City if available
    /// </summary>
    string City { get; }

    /// <summary>
    /// Postal code if available
    /// </summary>
    string PostalCode { get; }

    /// <summary>
    /// Estimated Latitude
    /// </summary>
    double Latitude { get; }

    /// <summary>
    /// Estimated Longitude
    /// </summary>
    double Longitude { get; }

    /// <summary>
    /// Metro code
    /// </summary>
    int MetroCode { get; }
  }
}
