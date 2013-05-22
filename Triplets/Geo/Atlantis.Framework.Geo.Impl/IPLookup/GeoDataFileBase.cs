using System;
using System.IO;

namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal abstract class GeoDataFileBase
  {
    protected static int COUNTRY_BEGIN = 16776960;
    protected static int MAX_RECORD_LENGTH = 4;

    private static int STRUCTURE_INFO_MAX_SIZE = 20;
    private static int SEGMENT_RECORD_LENGTH = 3;
    private static int STANDARD_RECORD_LENGTH = 3;
    private static int ORG_RECORD_LENGTH = 4;
    private static int STATE_BEGIN_REV0 = 16700000;
    private static int STATE_BEGIN_REV1 = 16000000;
    
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

    protected static String[] CountryNames = {
      "N/A","Asia/Pacific Region","Europe","Andorra","United Arab Emirates","Afghanistan","Antigua and Barbuda","Anguilla","Albania","Armenia","Curacao",
      "Angola","Antarctica","Argentina","American Samoa","Austria","Australia","Aruba","Azerbaijan","Bosnia and Herzegovina","Barbados",
      "Bangladesh","Belgium","Burkina Faso","Bulgaria","Bahrain","Burundi","Benin","Bermuda","Brunei Darussalam","Bolivia",
      "Brazil","Bahamas","Bhutan","Bouvet Island","Botswana","Belarus","Belize","Canada","Cocos (Keeling) Islands","Congo, The Democratic Republic of the",
      "Central African Republic","Congo","Switzerland","Cote D'Ivoire","Cook Islands","Chile","Cameroon","China","Colombia","Costa Rica",
      "Cuba","Cape Verde","Christmas Island","Cyprus","Czech Republic","Germany","Djibouti","Denmark","Dominica","Dominican Republic",
      "Algeria","Ecuador","Estonia","Egypt","Western Sahara","Eritrea","Spain","Ethiopia","Finland","Fiji",
      "Falkland Islands (Malvinas)","Micronesia, Federated States of","Faroe Islands","France","Sint Maarten (Dutch part)","Gabon","United Kingdom","Grenada","Georgia","French Guiana",
      "Ghana","Gibraltar","Greenland","Gambia","Guinea","Guadeloupe","Equatorial Guinea","Greece","South Georgia and the South Sandwich Islands","Guatemala",
      "Guam","Guinea-Bissau","Guyana","Hong Kong","Heard Island and McDonald Islands","Honduras","Croatia","Haiti","Hungary","Indonesia",
      "Ireland","Israel","India","British Indian Ocean Territory","Iraq","Iran, Islamic Republic of","Iceland","Italy","Jamaica","Jordan",
      "Japan","Kenya","Kyrgyzstan","Cambodia","Kiribati","Comoros","Saint Kitts and Nevis","Korea, Democratic People's Republic of","Korea, Republic of","Kuwait",
      "Cayman Islands","Kazakhstan","Lao People's Democratic Republic","Lebanon","Saint Lucia","Liechtenstein","Sri Lanka","Liberia","Lesotho","Lithuania",
      "Luxembourg","Latvia","Libya","Morocco","Monaco","Moldova, Republic of","Madagascar","Marshall Islands","Macedonia","Mali",
      "Myanmar","Mongolia","Macau","Northern Mariana Islands","Martinique","Mauritania","Montserrat","Malta","Mauritius","Maldives",
      "Malawi","Mexico","Malaysia","Mozambique","Namibia","New Caledonia","Niger","Norfolk Island","Nigeria","Nicaragua",
      "Netherlands","Norway","Nepal","Nauru","Niue","New Zealand","Oman","Panama","Peru","French Polynesia",
      "Papua New Guinea","Philippines","Pakistan","Poland","Saint Pierre and Miquelon","Pitcairn Islands","Puerto Rico","Palestinian Territory","Portugal","Palau",
      "Paraguay","Qatar","Reunion","Romania","Russian Federation","Rwanda","Saudi Arabia","Solomon Islands","Seychelles","Sudan",
      "Sweden","Singapore","Saint Helena","Slovenia","Svalbard and Jan Mayen","Slovakia","Sierra Leone","San Marino","Senegal","Somalia","Suriname",
      "Sao Tome and Principe","El Salvador","Syrian Arab Republic","Swaziland","Turks and Caicos Islands","Chad","French Southern Territories","Togo","Thailand",
      "Tajikistan","Tokelau","Turkmenistan","Tunisia","Tonga","Timor-Leste","Turkey","Trinidad and Tobago","Tuvalu","Taiwan",
      "Tanzania, United Republic of","Ukraine","Uganda","United States Minor Outlying Islands","United States","Uruguay","Uzbekistan","Holy See (Vatican City State)","Saint Vincent and the Grenadines","Venezuela",
      "Virgin Islands, British","Virgin Islands, U.S.","Vietnam","Vanuatu","Wallis and Futuna","Samoa","Yemen","Mayotte","Serbia","South Africa",
      "Zambia","Montenegro","Zimbabwe","Anonymous Proxy","Satellite Provider","Other","Aland Islands","Guernsey","Isle of Man","Jersey",
      "Saint Barthelemy","Saint Martin", "Bonaire, Saint Eustatius and Saba", "South Sudan", "Other" };

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
      }
    }

    protected static int UnsignedByteToInt(byte b)
    {
      return (int)b & 0xFF;
    }
  }

}
