using System;
using System.IO;

namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal abstract class GeoDataFileBase
  {
    protected const int COUNTRY_BEGIN = 16776960;
    protected const int MAX_RECORD_LENGTH = 4;
    protected const int FULL_RECORD_LENGTH = 100;

    const int STRUCTURE_INFO_MAX_SIZE = 20;
    const int SEGMENT_RECORD_LENGTH = 3;
    const int STANDARD_RECORD_LENGTH = 3;
    const int ORG_RECORD_LENGTH = 4;
    const int STATE_BEGIN_REV0 = 16700000;
    const int STATE_BEGIN_REV1 = 16000000;
    
    /// <summary>
    /// When updating this array from the maxmind API, 
    /// please remember to change GB to UK.
    /// </summary>
    protected static String[] CountryCodes = {
      "--","AP","EU","AD","AE","AF","AG","AI","AL","AM","CW",
      "AO","AQ","AR","AS","AT","AU","AW","AZ","BA","BB",
      "BD","BE","BF","BG","BH","BI","BJ","BM","BN","BO",
      "BR","BS","BT","BV","BW","BY","BZ","CA","CC","CD",
      "CF","CG","CH","CI","CK","CL","CM","CN","CO","CR",
      "CU","CV","CX","CY","CZ","DE","DJ","DK","DM","DO",
      "DZ","EC","EE","EG","EH","ER","ES","ET","FI","FJ",
      "FK","FM","FO","FR","SX","GA","UK","GD","GE","GF",
      "GH","GI","GL","GM","GN","GP","GQ","GR","GS","GT",
      "GU","GW","GY","HK","HM","HN","HR","HT","HU","ID",
      "IE","IL","IN","IO","IQ","IR","IS","IT","JM","JO",
      "JP","KE","KG","KH","KI","KM","KN","KP","KR","KW",
      "KY","KZ","LA","LB","LC","LI","LK","LR","LS","LT",
      "LU","LV","LY","MA","MC","MD","MG","MH","MK","ML",
      "MM","MN","MO","MP","MQ","MR","MS","MT","MU","MV",
      "MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI",
      "NL","NO","NP","NR","NU","NZ","OM","PA","PE","PF",
      "PG","PH","PK","PL","PM","PN","PR","PS","PT","PW",
      "PY","QA","RE","RO","RU","RW","SA","SB","SC","SD",
      "SE","SG","SH","SI","SJ","SK","SL","SM","SN","SO",
      "SR","ST","SV","SY","SZ","TC","TD","TF","TG","TH",
      "TJ","TK","TM","TN","TO","TL","TR","TT","TV","TW",
      "TZ","UA","UG","UM","US","UY","UZ","VA","VC","VE",
      "VG","VI","VN","VU","WF","WS","YE","YT","RS","ZA",
      "ZM","ME","ZW","A1","A2","O1","AX","GG","IM","JE",
      "BL","MF", "BQ", "SS", "O1"	};

    protected byte[] FileData { get; private set; }
    protected DatabaseInfo DataInfo { get; private set; }
    protected byte DatabaseType { get; private set; }

    int _recordLength = 0;
    protected int RecordLength
    {
      get { return _recordLength; }
    }

    int[] _databaseSegments;
    protected int[] DatabaseSegments
    {
      get { return _databaseSegments; }
    }

    public GeoDataFileBase(string filePath)
    {
      FileData = File.ReadAllBytes(filePath);
      Init();
    }

    private void Init()
    {
      int i, j;
      byte[] delim = new byte[3];
      byte[] buf = new byte[SEGMENT_RECORD_LENGTH];
      DatabaseType = Convert.ToByte(DatabaseInfo.COUNTRY_EDITION);
      _recordLength = STANDARD_RECORD_LENGTH;

      using (MemoryStream dataStream = new MemoryStream(FileData, false))
      {
        dataStream.Seek(-3, SeekOrigin.End);
        for (i = 0; i < STRUCTURE_INFO_MAX_SIZE; i++)
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
              _databaseSegments[0] = STATE_BEGIN_REV0;
              _recordLength = STANDARD_RECORD_LENGTH;
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
      }
    }

    protected int SeekCountry(long ipAddressNumber)
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

    protected static int UnsignedByteToInt(byte b)
    {
      return (int)b & 0xFF;
    }

    protected static long BytesToLong(byte[] address)
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
