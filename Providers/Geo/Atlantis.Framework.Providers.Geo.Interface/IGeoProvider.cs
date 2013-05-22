
namespace Atlantis.Framework.Providers.Geo.Interface
{
  public interface IGeoProvider
  {
    /// <summary>
    /// Returns the country code based on the IP address of the current request.
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
    /// Sets a spoofed IP address for the lifetime of the provider. This call will have no affect if 
    /// the request is not internal.
    /// </summary>
    /// <param name="spoofIpAddress">IP address to use as the users 'spoofed' IP</param>
    void SpoofUserIPAddress(string spoofIpAddress);
  }
}
