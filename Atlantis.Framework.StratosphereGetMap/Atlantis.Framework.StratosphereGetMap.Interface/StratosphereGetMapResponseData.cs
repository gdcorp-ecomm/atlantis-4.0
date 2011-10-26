using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.StratosphereGetMap.Interface
{
  public class StratosphereGetMapResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public StratosphereGetMapResponseData()
    { }

    public StratosphereGetMapResponseData(string xml)
    {
      if (!string.IsNullOrEmpty(xml))
      {
        _success = true;
        _resultXML = xml;
      }
    }

     public StratosphereGetMapResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public StratosphereGetMapResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "StratosphereGetMapResponseData",
                                   exception.Message,
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
    }
    #endregion
  }
}
