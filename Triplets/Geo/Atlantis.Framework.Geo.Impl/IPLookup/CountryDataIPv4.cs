using System;
using System.IO;
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

    private int SeekCountry(long ipAddressNumber)
    {
      byte[] buf = new byte[2 * MAX_RECORD_LENGTH];
      int[] x = new int[2];
      int offset = 0;
      for (int depth = 31; depth >= 0; depth--)
      {
        try
        {
          for (int i = 0; i < (2 * MAX_RECORD_LENGTH); i++)
          {
            buf[i] = FileData[i + (2 * RecordLength * offset)];
          }
        }
        catch (IOException)
        {
        }

        for (int i = 0; i < 2; i++)
        {
          x[i] = 0;
          for (int j = 0; j < RecordLength; j++)
          {
            int y = buf[(i * RecordLength) + j];
            if (y < 0)
            {
              y += 256;
            }
            x[i] += (y << (j * 8));
          }
        }

        if ((ipAddressNumber & (1 << depth)) > 0)
        {
          if (x[1] >= DatabaseSegments[0])
          {
            return x[1];
          }
          offset = x[1];
        }
        else
        {
          if (x[0] >= DatabaseSegments[0])
          {
            return x[0];
          }
          offset = x[0];
        }
      }

      return 0;
    }

    private static long BytesToLong(byte[] address)
    {
      long result = 0;
      for (int i = 0; i < 4; ++i)
      {
        long y = address[i];
        if (y < 0)
        {
          y += 256;
        }
        result += y << ((3 - i) * 8);
      }
      return result;
    }
  }

}
