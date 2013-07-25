using System.Collections.Generic;
namespace Atlantis.Framework.Providers.Geo.Interface
{
  /// <summary>
  /// Provides functionality related to GeoLocation, countrys, states, and regions
  /// </summary>
  public interface IGeoProvider
  {
    /// <summary>
    /// Returns the country code based on the IP address of the current request.
    /// Requires the GeoIP.dat country database
    /// </summary>
    string RequestCountryCode { get; }

    /// <summary>
    /// Checks to see if the current request is coming from the given country
    /// </summary>
    /// <param name="countryCode">country code to check</param>
    /// <returns>true if the country code of the current request matches the given country code (case-insenstive)</returns>
    bool IsUserInCountry(string countryCode);

    /// <summary>
    /// Checks to see if the current request is coming from a given region
    /// </summary>
    /// <param name="regionTypeId">Region type id (see lu_regionType table)</param>
    /// <param name="regionName">Region name (see lu_region table)</param>
    /// <returns>true if the country code of the current request is in the given region (case-insensitive)</returns>
    bool IsUserInRegion(int regionTypeId, string regionName);

    /// <summary>
    /// Returns the detailed GeoLocation based on the IP address of the current request.
    /// Requires the GeoIPLocation.dat city database
    /// </summary>
    IGeoLocation RequestGeoLocation { get; }

    /// <summary>
    /// Returns a collection of IGeoCountry objects
    /// </summary>
    IEnumerable<IGeoCountry> Countries { get; }

    /// <summary>
    /// Attempts to get the country by countryCode
    /// </summary>
    /// <param name="countryCode">countryCode</param>
    /// <param name="country">IGeoCountry object if found</param>
    /// <returns>true if found</returns>
    bool TryGetCountryByCode(string countryCode, out IGeoCountry country);

  }
}
