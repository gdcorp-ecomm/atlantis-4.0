using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.FTEAreaCodes.Interface
{
  public class Properties
  {
    #region Constants

    private const string ADMIN_NAME = "foswebapi";
    private const string REQUEST = "request";
    private const string REQUEST_TOKEN = "getToken";
    private const string REQUEST_STATES = "getGeocodeList";
    private const string REQUEST_AREACODES = "getGeoNDCList";
    private const string ADMIN = "admin_user_name";
    private const string STATUS = "status";
    private const string TIMESTAMP = "auth_timestamp";
    private const string TOKEN = "token";
    private const string HASH = "hash";
    private const string CC_CODE = "cc_code";
    private const string US_ONLY = "US";
    private const string SET = "set";
    private const string COUNT = "count";
    private const string LIST = "list";
    private const string GEOCODE = "geocode";
    private const string STATE_NAME = "name";

    private const int SET_INT = 1;
    private const int COUNT_INT = 50;

    #endregion

    #region Properties

    public Dictionary<string, object> FteProperties { get; set; }

    public Dictionary<string, string> ListedStates { get; set; }

    private Dictionary<string, object> _requestToken;
    public Dictionary<string, object> RequestToken
    {
      get
      {
        if (_requestToken == null)
        {
          _requestToken = GetRequestToken();
        }
        return _requestToken;
      }
    }

    private Dictionary<string, object> _requestStates;
    public Dictionary<string, object> RequestStates
    {
      get
      {
        if (_requestStates == null)
        {
          _requestStates = GetRequestStates();
        }
        return _requestStates;
      }
    }

    private Dictionary<string, object> _requestAreaCodes;
    public Dictionary<string, object> RequestAreaCodes
    {
      get
      {
        if (_requestAreaCodes == null)
        {
          _requestAreaCodes = GetRequestAreaCodes();
        }
        return _requestAreaCodes;
      }
    }

    private ArrayList _availableAreaCodes;
    public ArrayList AvailableAreaCodes 
    { 
      get
      {
        if (_availableAreaCodes == null)
        {
          _availableAreaCodes = SetAvailableAreaCodes();
        }
        return _availableAreaCodes;
      } 
    }

    public long Token { get; private set; }
    public string GeoCode { get; private set; }
    public string State { get; private set; }
    public string TimeStamp { get; private set; }
    public string Status { get; private set; }
    public string Hash { get; private set; }
    public string Password { get { return "fosweb@pi"; } }

    #endregion

    #region Methods

    public Properties() { }

    private Dictionary<string, object> GetRequestToken()
    {
      Dictionary<string, object> getRequestToken = new Dictionary<string, object>();

      getRequestToken.Add(REQUEST, REQUEST_TOKEN);
      getRequestToken.Add(ADMIN, ADMIN_NAME);

      return getRequestToken;
    }

    private Dictionary<string, object> GetRequestStates()
    {
      Dictionary<string, object> getRequestStates = new Dictionary<string, object>();

      getRequestStates.Add(REQUEST, REQUEST_STATES);
      getRequestStates.Add(HASH, Hash);
      getRequestStates.Add(TOKEN, Token);
      getRequestStates.Add(CC_CODE, US_ONLY);
      getRequestStates.Add(SET, SET_INT);
      getRequestStates.Add(COUNT, COUNT_INT);

      return getRequestStates;
    }

    private Dictionary<string, object> GetRequestAreaCodes()
    {
      Dictionary<string, object> getRequestAreaCodes = new Dictionary<string, object>();

      getRequestAreaCodes.Add(REQUEST, REQUEST_AREACODES);
      getRequestAreaCodes.Add(HASH, Hash);
      getRequestAreaCodes.Add(TOKEN, Token);
      getRequestAreaCodes.Add(GEOCODE, GeoCode);
      getRequestAreaCodes.Add(SET, SET_INT);
      getRequestAreaCodes.Add(COUNT, COUNT_INT);

      return getRequestAreaCodes;
    }

    public void GetTokenProps()
    {
      Status = FteProperties[STATUS].ToString();
      TimeStamp = FteProperties[TIMESTAMP].ToString();

      long result = 0;
      if (long.TryParse(FteProperties[TOKEN].ToString(), out result))
      {
        Token = result;
      }

      GetHash();
    }

    public void GetStateProps()
    {
      var jsSerializer = new JavaScriptSerializer();
      var json = jsSerializer.Serialize(FteProperties[LIST]);      

      ArrayList arrJson = jsSerializer.Deserialize<ArrayList>(json);

      ListedStates = new Dictionary<string, string>();

      foreach (Dictionary<string, object> item in arrJson)
      {
        ListedStates.Add(item[GEOCODE].ToString(), item[STATE_NAME].ToString());
      }
    }

    public void GetAreaCodeProps(string geoCode)
    {
      GeoCode = geoCode;
    }

    public void GetHash()
    {
      SHA1 sha1Provider = new SHA1CryptoServiceProvider();

      byte[] passwordHash = sha1Provider.ComputeHash(Encoding.ASCII.GetBytes(Password));
      string pwHashString = BitConverter.ToString(passwordHash).Replace("-", string.Empty);

      string data = string.Concat(pwHashString, TimeStamp, Token.ToString()).ToLowerInvariant();
      byte[] hashedData = sha1Provider.ComputeHash(Encoding.ASCII.GetBytes(data));

      Hash = BitConverter.ToString(hashedData).Replace("-", string.Empty).ToLowerInvariant();
    }

    public ArrayList SetAvailableAreaCodes()
    {
      var jsSerializer = new JavaScriptSerializer();
      var json = jsSerializer.Serialize(FteProperties[LIST]);

      ArrayList arrJson = jsSerializer.Deserialize<ArrayList>(json);

      return arrJson;
    }

    #endregion
  }
}
