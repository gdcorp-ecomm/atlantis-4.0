using System;
using System.IO;
using System.Net;
using Atlantis.Framework.GeoByIP.Interface;

namespace Atlantis.Framework.GeoByIP.Impl
{
  internal abstract class GeoDataFileBase
  {
    protected const int COUNTRY_BEGIN = 16776960;
    protected const int STATE_BEGIN = 16700000;
    protected const int STRUCTURE_INFO_MAX_SIZE = 20;
    protected const int DATABASE_INFO_MAX_SIZE = 100;
    protected const int FULL_RECORD_LENGTH = 100;//???
    protected const int SEGMENT_RECORD_LENGTH = 3;
    protected const int STANDARD_RECORD_LENGTH = 3;
    protected const int ORG_RECORD_LENGTH = 4;
    protected const int MAX_RECORD_LENGTH = 4;
    protected const int MAX_ORG_RECORD_LENGTH = 1000;//???
    protected const int FIPS_RANGE = 360;
    protected const int STATE_BEGIN_REV0 = 16700000;
    protected const int STATE_BEGIN_REV1 = 16000000;
    protected const int US_OFFSET = 1;
    protected const int CANADA_OFFSET = 677;
    protected const int WORLD_OFFSET = 1353;
    protected const int GEOIP_STANDARD = 0;
    protected const int GEOIP_MEMORY_CACHE = 1;
    protected const int GEOIP_UNKNOWN_SPEED = 0;
    protected const int GEOIP_DIALUP_SPEED = 1;
    protected const int GEOIP_CABLEDSL_SPEED = 2;
    protected const int GEOIP_CORPORATE_SPEED = 3;

    protected static string[] CountryCodes = {
      "--","AP","EU","AD","AE","AF","AG","AI","AL","AM","AN","AO","AQ","AR",
      "AS","AT","AU","AW","AZ","BA","BB","BD","BE","BF","BG","BH","BI","BJ",
      "BM","BN","BO","BR","BS","BT","BV","BW","BY","BZ","CA","CC","CD","CF",
      "CG","CH","CI","CK","CL","CM","CN","CO","CR","CU","CV","CX","CY","CZ",
      "DE","DJ","DK","DM","DO","DZ","EC","EE","EG","EH","ER","ES","ET","FI",
      "FJ","FK","FM","FO","FR","FX","GA","GB","GD","GE","GF","GH","GI","GL",
      "GM","GN","GP","GQ","GR","GS","GT","GU","GW","GY","HK","HM","HN","HR",
      "HT","HU","ID","IE","IL","IN","IO","IQ","IR","IS","IT","JM","JO","JP",
      "KE","KG","KH","KI","KM","KN","KP","KR","KW","KY","KZ","LA","LB","LC",
      "LI","LK","LR","LS","LT","LU","LV","LY","MA","MC","MD","MG","MH","MK",
      "ML","MM","MN","MO","MP","MQ","MR","MS","MT","MU","MV","MW","MX","MY",
      "MZ","NA","NC","NE","NF","NG","NI","NL","NO","NP","NR","NU","NZ","OM",
      "PA","PE","PF","PG","PH","PK","PL","PM","PN","PR","PS","PT","PW","PY",
      "QA","RE","RO","RU","RW","SA","SB","SC","SD","SE","SG","SH","SI","SJ",
      "SK","SL","SM","SN","SO","SR","ST","SV","SY","SZ","TC","TD","TF","TG",
      "TH","TJ","TK","TM","TN","TO","TL","TR","TT","TV","TW","TZ","UA","UG",
      "UM","US","UY","UZ","VA","VC","VE","VG","VI","VN","VU","WF","WS","YE",
      "YT","RS","ZA","ZM","ME","ZW","A1","A2","O1","AX","GG","IM","JE","BL",
	    "MF"};

    protected static string[] CountryNames = {
      "N/A","Asia/Pacific Region","Europe","Andorra","United Arab Emirates",
      "Afghanistan","Antigua and Barbuda","Anguilla","Albania","Armenia",
      "Netherlands Antilles","Angola","Antarctica","Argentina","American Samoa",
      "Austria","Australia","Aruba","Azerbaijan","Bosnia and Herzegovina",
      "Barbados","Bangladesh","Belgium","Burkina Faso","Bulgaria","Bahrain",
      "Burundi","Benin","Bermuda","Brunei Darussalam","Bolivia","Brazil","Bahamas",
      "Bhutan","Bouvet Island","Botswana","Belarus","Belize","Canada",
      "Cocos (Keeling) Islands","Congo, The Democratic Republic of the",
      "Central African Republic","Congo","Switzerland","Cote D'Ivoire",
      "Cook Islands","Chile","Cameroon","China","Colombia","Costa Rica","Cuba",
      "Cape Verde","Christmas Island","Cyprus","Czech Republic","Germany",
      "Djibouti","Denmark","Dominica","Dominican Republic","Algeria","Ecuador",
      "Estonia","Egypt","Western Sahara","Eritrea","Spain","Ethiopia","Finland",
      "Fiji","Falkland Islands (Malvinas)","Micronesia, Federated States of",
      "Faroe Islands","France","France, Metropolitan","Gabon","United Kingdom",
      "Grenada","Georgia","French Guiana","Ghana","Gibraltar","Greenland","Gambia",
      "Guinea","Guadeloupe","Equatorial Guinea","Greece",
      "South Georgia and the South Sandwich Islands","Guatemala","Guam",
      "Guinea-Bissau","Guyana","Hong Kong","Heard Island and McDonald Islands",
      "Honduras","Croatia","Haiti","Hungary","Indonesia","Ireland","Israel","India",
      "British Indian Ocean Territory","Iraq","Iran, Islamic Republic of",
      "Iceland","Italy","Jamaica","Jordan","Japan","Kenya","Kyrgyzstan","Cambodia",
      "Kiribati","Comoros","Saint Kitts and Nevis",
      "Korea, Democratic People's Republic of","Korea, Republic of","Kuwait",
      "Cayman Islands","Kazakhstan","Lao People's Democratic Republic","Lebanon",
      "Saint Lucia","Liechtenstein","Sri Lanka","Liberia","Lesotho","Lithuania",
      "Luxembourg","Latvia","Libyan Arab Jamahiriya","Morocco","Monaco",
      "Moldova, Republic of","Madagascar","Marshall Islands",
      "Macedonia, the Former Yugoslav Republic of","Mali","Myanmar","Mongolia",
      "Macau","Northern Mariana Islands","Martinique","Mauritania","Montserrat",
      "Malta","Mauritius","Maldives","Malawi","Mexico","Malaysia","Mozambique",
      "Namibia","New Caledonia","Niger","Norfolk Island","Nigeria","Nicaragua",
      "Netherlands","Norway","Nepal","Nauru","Niue","New Zealand","Oman","Panama",
      "Peru","French Polynesia","Papua New Guinea","Philippines","Pakistan",
      "Poland","Saint Pierre and Miquelon","Pitcairn","Puerto Rico","" +
      "Palestinian Territory, Occupied","Portugal","Palau","Paraguay","Qatar",
      "Reunion","Romania","Russian Federation","Rwanda","Saudi Arabia",
      "Solomon Islands","Seychelles","Sudan","Sweden","Singapore","Saint Helena",
      "Slovenia","Svalbard and Jan Mayen","Slovakia","Sierra Leone","San Marino",
      "Senegal","Somalia","Suriname","Sao Tome and Principe","El Salvador",
      "Syrian Arab Republic","Swaziland","Turks and Caicos Islands","Chad",
      "French Southern Territories","Togo","Thailand","Tajikistan","Tokelau",
      "Turkmenistan","Tunisia","Tonga","Timor-Leste","Turkey","Trinidad and Tobago",
      "Tuvalu","Taiwan","Tanzania, United Republic of","Ukraine","Uganda",
      "United States Minor Outlying Islands","United States","Uruguay","Uzbekistan",
      "Holy See (Vatican City State)","Saint Vincent and the Grenadines",
      "Venezuela","Virgin Islands, British","Virgin Islands, U.S.","Vietnam",
      "Vanuatu","Wallis and Futuna","Samoa","Yemen","Mayotte","Serbia",
      "South Africa","Zambia","Montenegro","Zimbabwe","Anonymous Proxy",
      "Satellite Provider","Other",
	    "Aland Islands","Guernsey","Isle of Man","Jersey","Saint Barthelemy",
	    "Saint Martin"};

    protected byte[] FileData { get; private set; }
    protected DatabaseInfo DataInfo { get; private set; }
    protected byte DatabaseType { get; private set; }

    int _options;
    int[] _databaseSegments;
    int _recordLength;
    byte[] _dbbuffer;

    public GeoDataFileBase(string filePath, int options)
    {
      FileData = File.ReadAllBytes(filePath);
      _options = options;
      Init();
    }

    private void Init()
    {
      int i, j;
      byte[] delim = new byte[3];
      byte[] buf = new byte[GeoDataFileBase.SEGMENT_RECORD_LENGTH];
      DatabaseType = Convert.ToByte(DatabaseInfo.COUNTRY_EDITION);
      _recordLength = GeoDataFileBase.STANDARD_RECORD_LENGTH;

      using (MemoryStream dataStream = new MemoryStream(FileData, false))
      {
        dataStream.Seek(-3, SeekOrigin.End);
        for (i = 0; i < GeoDataFileBase.STRUCTURE_INFO_MAX_SIZE; i++)
        {
          dataStream.Read(delim, 0, 3);
          if (delim[0] == 255 && delim[1] == 255 && delim[2] == 255)
          {
            DatabaseType = Convert.ToByte(dataStream.ReadByte());
            if (DatabaseType >= 106)
            {
              // Backward compatibility with databases from April 2003 and earlier
              DatabaseType -= 105;
            }
            // Determine the database type.
            if (DatabaseType == DatabaseInfo.REGION_EDITION_REV0)
            {
              _databaseSegments = new int[1];
              _databaseSegments[0] = GeoDataFileBase.STATE_BEGIN_REV0;
              _recordLength = GeoDataFileBase.STANDARD_RECORD_LENGTH;
            }
            else if (DatabaseType == DatabaseInfo.REGION_EDITION_REV1)
            {
              _databaseSegments = new int[1];
              _databaseSegments[0] = STATE_BEGIN_REV1;
              _recordLength = STANDARD_RECORD_LENGTH;
            }
            else if (
              DatabaseType == DatabaseInfo.CITY_EDITION_REV0 ||
              DatabaseType == DatabaseInfo.CITY_EDITION_REV1 ||
              DatabaseType == DatabaseInfo.ORG_EDITION ||
              DatabaseType == DatabaseInfo.ORG_EDITION_V6 ||
              DatabaseType == DatabaseInfo.ISP_EDITION ||
              DatabaseType == DatabaseInfo.ISP_EDITION_V6 ||
              DatabaseType == DatabaseInfo.ASNUM_EDITION ||
              DatabaseType == DatabaseInfo.ASNUM_EDITION_V6 ||
              DatabaseType == DatabaseInfo.NETSPEED_EDITION_REV1 ||
              DatabaseType == DatabaseInfo.NETSPEED_EDITION_REV1_V6 ||
              DatabaseType == DatabaseInfo.CITY_EDITION_REV0_V6 ||
              DatabaseType == DatabaseInfo.CITY_EDITION_REV1_V6)
            {
              _databaseSegments = new int[1];
              _databaseSegments[0] = 0;
              if (
                DatabaseType == DatabaseInfo.CITY_EDITION_REV0 ||
                DatabaseType == DatabaseInfo.CITY_EDITION_REV1 ||
                DatabaseType == DatabaseInfo.ASNUM_EDITION_V6 ||
                DatabaseType == DatabaseInfo.NETSPEED_EDITION_REV1 ||
                DatabaseType == DatabaseInfo.NETSPEED_EDITION_REV1_V6 ||
                DatabaseType == DatabaseInfo.CITY_EDITION_REV0_V6 ||
                DatabaseType == DatabaseInfo.CITY_EDITION_REV1_V6 ||
                DatabaseType == DatabaseInfo.ASNUM_EDITION)
              {
                _recordLength = STANDARD_RECORD_LENGTH;
              }
              else
              {
                _recordLength = ORG_RECORD_LENGTH;
              }
              dataStream.Read(buf, 0, SEGMENT_RECORD_LENGTH);
              for (j = 0; j < SEGMENT_RECORD_LENGTH; j++)
              {
                _databaseSegments[0] += (UnsignedByteToInt(buf[j]) << (j * 8));
              }
            }
            break;
          }
          else
          {
            dataStream.Seek(-4, SeekOrigin.Current);
          }
        }
        if (
          (DatabaseType == DatabaseInfo.COUNTRY_EDITION) ||
          (DatabaseType == DatabaseInfo.COUNTRY_EDITION_V6) ||
          (DatabaseType == DatabaseInfo.PROXY_EDITION) ||
          (DatabaseType == DatabaseInfo.NETSPEED_EDITION))
        {
          _databaseSegments = new int[1];
          _databaseSegments[0] = COUNTRY_BEGIN;
          _recordLength = STANDARD_RECORD_LENGTH;
        }
        if ((_options & GEOIP_MEMORY_CACHE) == 1)
        {
          int l = (int)dataStream.Length;
          _dbbuffer = new byte[l];
          dataStream.Seek(0, SeekOrigin.Begin);
          dataStream.Read(_dbbuffer, 0, l);
        }
      }
    }

    protected static int UnsignedByteToInt(byte b)
    {
      return (int)b & 0xFF;
    }

    protected int SeekCountryV6(IPAddress address)
    {
      return SeekCountryV6(address, null);
    }

    protected int SeekCountryV6(IPAddress address, MemoryStream dataStream)
    {
      byte[] v6vec = address.GetAddressBytes();
      byte[] buf = new byte[2 * MAX_RECORD_LENGTH];
      int[] x = new int[2];
      int offset = 0;
      for (int depth = 127; depth >= 0; depth--)
      {
        if ((_options & GEOIP_MEMORY_CACHE) == 1)
        {
          for (int i = 0; i < (2 * MAX_RECORD_LENGTH); i++)
          {
            buf[i] = _dbbuffer[i + (2 * _recordLength * offset)];
          }
        }
        else
        {
          if (dataStream != null)
          {
            dataStream.Seek(2 * _recordLength * offset, SeekOrigin.Begin);
            dataStream.Read(buf, 0, 2 * MAX_RECORD_LENGTH);
          }
          else
          {
            using (MemoryStream newStream = new MemoryStream(FileData))
            {
              newStream.Seek(2 * _recordLength * offset, SeekOrigin.Begin);
              newStream.Read(buf, 0, 2 * MAX_RECORD_LENGTH);
            }
          }
        }

        for (int i = 0; i < 2; i++)
        {
          x[i] = 0;
          for (int j = 0; j < _recordLength; j++)
          {
            int y = buf[(i * _recordLength) + j];
            if (y < 0)
            {
              y += 256;
            }
            x[i] += (y << (j * 8));
          }
        }

        int bnum = 127 - depth;
        int idx = bnum >> 3;
        int b_mask = 1 << (bnum & 7 ^ 7);
        if ((v6vec[idx] & b_mask) > 0)
        {
          if (x[1] >= _databaseSegments[0])
          {
            return x[1];
          }
          offset = x[1];
        }
        else
        {
          if (x[0] >= _databaseSegments[0])
          {
            return x[0];
          }
          offset = x[0];
        }
      }

      return 0;
    }

    protected GeoLocation GetLocationV6(IPAddress address)
    {
      int record_pointer;
      byte[] record_buf = new byte[FULL_RECORD_LENGTH];
      char[] record_buf2 = new char[FULL_RECORD_LENGTH];
      int record_buf_offset = 0;
      GeoLocation result = null;
      int str_length = 0;
      int j, seek_country;
      double latitude = 0, longitude = 0;

      using (MemoryStream dataStream = new MemoryStream(FileData))
      {
        seek_country = SeekCountryV6(address, dataStream);
        if (seek_country == _databaseSegments[0])
        {
          return null;
        }

        record_pointer = seek_country + ((2 * _recordLength - 1) * _databaseSegments[0]);
        if ((_options & GEOIP_MEMORY_CACHE) == 1)
        {
          Array.Copy(_dbbuffer, record_pointer, record_buf, 0, Math.Min(_dbbuffer.Length - record_pointer, FULL_RECORD_LENGTH));
        }
        else
        {
          dataStream.Seek(record_pointer, SeekOrigin.Begin);
          dataStream.Read(record_buf, 0, FULL_RECORD_LENGTH);
        }
      }

      for (int a0 = 0; a0 < FULL_RECORD_LENGTH; a0++)
      {
        record_buf2[a0] = Convert.ToChar(record_buf[a0]);
      }

      int countryIndex = UnsignedByteToInt(record_buf[0]);
      GeoCountry country = new GeoCountry(CountryCodes[countryIndex], CountryNames[countryIndex]);
      result = new GeoLocation(country);

      record_buf_offset++;

      // get region
      while (record_buf[record_buf_offset + str_length] != '\0')
      {
        str_length++;
      }

      if (str_length > 0)
      {
        result.Region = new string(record_buf2, record_buf_offset, str_length);
      }
      record_buf_offset += str_length + 1;
      str_length = 0;

      // get region_name
      result.RegionName = RegionName.GetRegionName(result.Country.Code, result.Region);

      // get city
      while (record_buf[record_buf_offset + str_length] != '\0')
      {
        str_length++;
      }

      if (str_length > 0)
      {
        result.City = new string(record_buf2, record_buf_offset, str_length);
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
        result.PostalCode = new string(record_buf2, record_buf_offset, str_length);
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
      result.DMACode = 0;
      result.AreaCode = 0;

      if (DatabaseType == DatabaseInfo.CITY_EDITION_REV1 || DatabaseType == DatabaseInfo.CITY_EDITION_REV1_V6)
      {
        // get metro_code
        int metroarea_combo = 0;
        if ("US".Equals(result.Country.Code, StringComparison.InvariantCultureIgnoreCase))
        {
          record_buf_offset += 3;
          for (j = 0; j < 3; j++)
          {
            metroarea_combo += (UnsignedByteToInt(record_buf[record_buf_offset + j]) << (j * 8));
          }

          result.DMACode = metroarea_combo / 1000;
          result.MetroCode = result.DMACode;
          result.AreaCode = metroarea_combo % 1000;
        }
      }

      return result;
    }
  }
}
