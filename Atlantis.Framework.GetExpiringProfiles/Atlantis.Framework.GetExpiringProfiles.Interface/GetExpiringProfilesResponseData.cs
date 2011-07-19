using System;
using Atlantis.Framework.Interface;
using System.IO;
using System.Xml.Linq;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.GetExpiringProfiles.Interface
{
  public class GetExpiringProfilesResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private int _daysUntilProfileExpires = int.MaxValue;
    private bool _hasExpiringOrExpiredProfile = false;    

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public int DaysUntilProfileExpires
    {
      get
      {
        return _daysUntilProfileExpires;
      }     
    }

    public bool HasExpiringProfile
    {
      get { return _hasExpiringOrExpiredProfile && (_daysUntilProfileExpires > 0); }
    }

    public bool HasExpiredProfile
    {
      get { return _hasExpiringOrExpiredProfile && (_daysUntilProfileExpires <= 0); }
    }
    #endregion

    public GetExpiringProfilesResponseData()
    { }

    public GetExpiringProfilesResponseData(RequestData requestData, string resultXML)
    {
      _resultXML = resultXML;
      ParseResult();
    }

    private void ParseResult()
    {
      if (string.IsNullOrEmpty(_resultXML))
      {
        _success = false;
      }
      else
      {
        XElement resultElement = XElement.Parse(_resultXML);
        XAttribute expirationAttribute = resultElement.Attribute("earliest_expiration");
        if (expirationAttribute != null)
        {
          DateTime expiration;
          if (DateTime.TryParse(expirationAttribute.Value, out expiration))
          {
            _hasExpiringOrExpiredProfile = true;
            _daysUntilProfileExpires = (int)Math.Round((expiration - DateTime.Now).TotalDays);
          }
        }
        _success = true;
      }
    }

    public GetExpiringProfilesResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public GetExpiringProfilesResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData,
                                   "GetExpiringProfilesResponseData",
                                   ex.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion


    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return _resultXML;
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultXML = sessionData;
      ParseResult();
    }

    #endregion
  }
}
