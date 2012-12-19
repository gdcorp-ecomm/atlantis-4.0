using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.FTE.Interface
{
  public class Properties
  {
    public class StatesCollection
    {
      public string geocode { get; set; }
      public string name { get; set; }
    }

    public Dictionary<string, object> FteProperties { get; set; }
    public Dictionary<string, string> ListedStates { get; set; }
    public long Token { get; private set; }
    public string StateGeoCode { get; private set; }
    public string TimeStamp { get; private set; }
    public string Status { get; private set; }
    public string Hash { get; private set; }
    public string Password { get; private set; }
    public string Admin { get; private set; }
    public string CcCode { get; private set; }

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

    private List<string> _availableAreaCodes;
    public List<string> AvailableAreaCodes
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

    public Properties() { }

    private Dictionary<string, object> GetRequestToken()
    {
      Dictionary<string, object> getRequestToken = new Dictionary<string, object>();

      getRequestToken.Add(Constants.REQUEST, Constants.REQUEST_TOKEN);
      getRequestToken.Add(Constants.ADMIN, Admin);

      return getRequestToken;
    }

    private Dictionary<string, object> GetRequestStates()
    {
      Dictionary<string, object> getRequestStates = new Dictionary<string, object>();

      getRequestStates.Add(Constants.REQUEST, Constants.REQUEST_STATES);
      getRequestStates.Add(Constants.HASH, Hash);
      getRequestStates.Add(Constants.TOKEN, Token);
      getRequestStates.Add(Constants.CC_CODE, CcCode);
      getRequestStates.Add(Constants.SET, Constants.SET_INT);
      getRequestStates.Add(Constants.COUNT, Constants.COUNT_INT);

      return getRequestStates;
    }

    private Dictionary<string, object> GetRequestAreaCodes()
    {
      Dictionary<string, object> getRequestAreaCodes = new Dictionary<string, object>();

      getRequestAreaCodes.Add(Constants.REQUEST, Constants.REQUEST_AREACODES);
      getRequestAreaCodes.Add(Constants.HASH, Hash);
      getRequestAreaCodes.Add(Constants.TOKEN, Token);
      getRequestAreaCodes.Add(Constants.GEOCODE, StateGeoCode);
      getRequestAreaCodes.Add(Constants.SET, Constants.SET_INT);
      getRequestAreaCodes.Add(Constants.COUNT, Constants.COUNT_INT);

      return getRequestAreaCodes;
    }

    public void GetLoginCred(string admin, string password)
    {
      Admin = admin;
      Password = password;
    }

    public void GetTokenProps()
    {
      Status = FteProperties[Constants.STATUS].ToString();
      TimeStamp = FteProperties[Constants.TIMESTAMP].ToString();

      long result = 0;
      if (long.TryParse(FteProperties[Constants.TOKEN].ToString(), out result))
      {
        Token = result;
      }

      GetHash();
    }

    public void GetStateProps()
    {
      var jsSerializer = new JavaScriptSerializer();
      var json = jsSerializer.Serialize(FteProperties[Constants.LIST]);

      IList<StatesCollection> collection = new JavaScriptSerializer().Deserialize<IList<StatesCollection>>(json);

      ListedStates = new Dictionary<string, string>();

      foreach (StatesCollection item in collection)
      {
        ListedStates.Add(item.geocode, item.name);
      }
    }

    public void GetAreaCodeProps(string geoCode)
    {
      StateGeoCode = geoCode;
    }

    internal void GetStateProps(string ccCode)
    {
      CcCode = ccCode;
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

    public List<string> SetAvailableAreaCodes()
    {
      var jsSerializer = new JavaScriptSerializer();
      var json = jsSerializer.Serialize(FteProperties[Constants.LIST]);

      List<string> listJson = jsSerializer.Deserialize<List<string>>(json);

      return listJson;
    }
  }
}