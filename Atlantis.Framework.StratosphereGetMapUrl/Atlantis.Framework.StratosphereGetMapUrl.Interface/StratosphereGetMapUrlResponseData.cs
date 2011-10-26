using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.StratosphereGetMapUrl.Interface
{
  public class StratosphereGetMapUrlResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultUrl = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public StratosphereGetMapUrlResponseData()
    {
    }

    public StratosphereGetMapUrlResponseData(string url)
    {
      _resultUrl = url;
      _success = true;
    }

     public StratosphereGetMapUrlResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public StratosphereGetMapUrlResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "StratosphereGetMapUrlResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultUrl;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return _resultUrl;
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultUrl = sessionData;
    }
    #endregion
  }
}
