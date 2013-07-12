using Atlantis.Framework.Geo.Interface;
using System;
using System.IO;
using System.Net;

namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal class LocationDataIPv4 : GeoDataFileBase
  {
    public LocationDataIPv4(string filePath)
      : base(filePath)
    {
      if (DatabaseType != DatabaseInfo.CITY_EDITION_REV1)
      {
        throw new Exception("City file is not valid format: " + DatabaseType.ToString() + ": expected " + DatabaseInfo.CITY_EDITION_REV1.ToString());
      }
    }

    public IPLocation GetLocation(string ipAddress)
    {
      IPLocation result = InternalCountries.LookupLocation(ipAddress);

      if (result == null)
      {
        IPAddress address;
        if (IPAddress.TryParse(ipAddress, out address))
        {
          result = GetLocation(address);
        }
      }

      return result;
    }

    private IPLocation GetLocation(IPAddress address)
    {
      return GetLocation(BytesToLong(address.GetAddressBytes()));
    }

    private IPLocation GetLocation(long ipAddressNumber)
    {
      IPLocation result = new IPLocation();

      int record_pointer;
      byte[] record_buf = new byte[FULL_RECORD_LENGTH];
      char[] record_buf2 = new char[FULL_RECORD_LENGTH];
      int record_buf_offset = 0;
      int str_length = 0;
      int j, Seek_country;
      double latitude = 0, longitude = 0;

      try
      {
        Seek_country = SeekCountry(ipAddressNumber);
        if (Seek_country == DatabaseSegments[0])
        {
          return null;
        }

        record_pointer = Seek_country + ((2 * RecordLength - 1) * DatabaseSegments[0]);
        Array.Copy(FileData, record_pointer, record_buf, 0, Math.Min(FileData.Length - record_pointer, FULL_RECORD_LENGTH));

        for (int a0 = 0; a0 < FULL_RECORD_LENGTH; a0++)
        {
          record_buf2[a0] = Convert.ToChar(record_buf[a0]);
        }

        // get country
        result.CountryCode = CountryCodes[UnsignedByteToInt(record_buf[0])];
        record_buf_offset++;

        // get region
        while (record_buf[record_buf_offset + str_length] != '\0')
        {
          str_length++;
        }

        if (str_length > 0)
        {
          result.Region = new String(record_buf2, record_buf_offset, str_length);
        }
        record_buf_offset += str_length + 1;
        str_length = 0;

        // get region_name
        result.RegionName = RegionName.GetRegionName(result.CountryCode, result.Region);

        // get city
        while (record_buf[record_buf_offset + str_length] != '\0')
        {
          str_length++;
        }

        if (str_length > 0)
        {
          result.City = new String(record_buf2, record_buf_offset, str_length);
        }
        record_buf_offset += (str_length + 1);
        str_length = 0;

        // get postal code
        while (record_buf[record_buf_offset + str_length] != '\0')
        {
          str_length++;
        }

        if (str_length > 0)
        {
          result.PostalCode = new String(record_buf2, record_buf_offset, str_length);
        }
        record_buf_offset += (str_length + 1);

        // get latitude
        for (j = 0; j < 3; j++)
        {
          latitude += (UnsignedByteToInt(record_buf[record_buf_offset + j]) << (j * 8));
        }
        result.Latitude = (float)latitude / 10000 - 180;
        record_buf_offset += 3;

        // get longitude
        for (j = 0; j < 3; j++)
        {
          longitude += (UnsignedByteToInt(record_buf[record_buf_offset + j]) << (j * 8));
        }
        result.Longitude = (float)longitude / 10000 - 180;

        result.MetroCode = 0;
        
        if (DatabaseType == DatabaseInfo.CITY_EDITION_REV1)
        {
          // get metro_code
          int metroarea_combo = 0;
          if ("US".Equals(result.CountryCode, StringComparison.OrdinalIgnoreCase))
          {
            record_buf_offset += 3;
            for (j = 0; j < 3; j++)
            {
              metroarea_combo += (UnsignedByteToInt(record_buf[record_buf_offset + j]) << (j * 8));
            }

            result.MetroCode = metroarea_combo / 1000;
          }
        }
      }
      catch (IOException)
      {
      }

      return result;
    }


  }
}
