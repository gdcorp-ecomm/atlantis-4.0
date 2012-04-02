using System;
using System.Net;
using Atlantis.Framework.GeoByIP.Interface;

namespace Atlantis.Framework.GeoByIP.Impl
{
  internal class CountryData : GeoDataFileBase
  {
    // File MUST be the country IPv6 dat format
    public CountryData(string filePath)
      : base(filePath, GEOIP_MEMORY_CACHE)
    { }

    public GeoCountry GetCountry(string ipAddress)
    {
      GeoCountry result = GeoCountry.UnknownCountry;

      IPAddress address;
      if (IPAddress.TryParse(ipAddress, out address))
      {
        result = GetCountryV6(address);
      }

      return result;
    }

    private GeoCountry GetCountryV6(IPAddress address)
    {
      if (DatabaseType == DatabaseInfo.CITY_EDITION_REV0 || DatabaseType == DatabaseInfo.CITY_EDITION_REV1)
      {
        throw new InvalidOperationException("Country Lookup dat file cannot use the CITY_EDITION dat file.");
      }

      int countryIndex = SeekCountryV6(address) - COUNTRY_BEGIN;
      if (countryIndex == 0)
      {
        return GeoCountry.UnknownCountry;
      }
      else
      {
        return new GeoCountry(CountryCodes[countryIndex], CountryNames[countryIndex]);
      }
    }
  }
}
