using System;

using Atlantis.Framework.Interface;


namespace Atlantis.Framework.GetMobileToken.Interface
{
  public class GetMobileTokenResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private string _sessionToken = null;

    public GetMobileTokenResponseData(string sessionToken)
    {
      _sessionToken = sessionToken;
      _success = true;
    }

    public GetMobileTokenResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GetMobileTokenResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetMobileTokenResponseData",
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

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public string SessionToken
    {
      get
      {
        return _sessionToken;
      }
    }
    #endregion

  }
}

