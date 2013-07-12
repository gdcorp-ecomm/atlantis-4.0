using System;
using System.Net;

namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal class CountryDataIPv4 : GeoDataFileBase
  {
    public CountryDataIPv4(string filePath)
      : base(filePath)
    {
      if (DatabaseType != DatabaseInfo.COUNTRY_EDITION)
      {
        throw new Exception("Country file is not valid format: " + DatabaseType.ToString() + ": expected " + DatabaseInfo.COUNTRY_EDITION.ToString());
      }
    }

    public string GetCountry(string ipAddress)
    {
      string result = InternalCountries.LookupCountryCode(ipAddress);

      if (string.IsNullOrEmpty(result))
      {
        IPAddress address;
        if (IPAddress.TryParse(ipAddress, out address))
        {
          result = GetCountryCode(address);
        }
      }

      return result;
    }

    private string GetCountryCode(IPAddress address)
    {
      string result = string.Empty;

      int countryIndex = SeekCountry(address) - COUNTRY_BEGIN;
      if ((countryIndex > 0) && (countryIndex < CountryCodes.Length))
      {
        result = CountryCodes[countryIndex];
      }

      return result;
    }

    private int SeekCountry(IPAddress address)
    {
      long ipAddressNumber = BytesToLong(address.GetAddressBytes());
      return SeekCountry(ipAddressNumber);
    }
  }

}
