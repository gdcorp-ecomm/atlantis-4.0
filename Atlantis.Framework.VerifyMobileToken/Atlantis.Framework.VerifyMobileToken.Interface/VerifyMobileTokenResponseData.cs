using System;

using Atlantis.Framework.Interface;


namespace Atlantis.Framework.VerifyMobileToken.Interface
{
  public class VerifyMobileTokenResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private bool _isValid = false;
    private string _sessionToken = null;

    public VerifyMobileTokenResponseData(bool isValid, String sessionToken)
    {
      _success      = true;
      _isValid      = isValid;
      _sessionToken = sessionToken;
    }

    public VerifyMobileTokenResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public VerifyMobileTokenResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "VerifyMobileTokenResponseData",
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

    public bool IsValid
    {
      get 
      {
        return _isValid;
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

