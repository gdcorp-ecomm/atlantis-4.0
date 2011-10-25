using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAddDomain.Interface
{
  public class HCCAddDomainResponseData : IResponseData, IHCCResponseMessage
  {
    private readonly AtlantisException _exception = null;
    private string _resultXml = string.Empty;
    private readonly bool _success = false;
    private string _status;
    private int _statusCode;
    private string _message;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }
    
    public HCCAddDomainResponseData(string message, string status, int statusCode)
    {
      _success = true;
      _message = message;
      _status = status;
      _statusCode = statusCode;
    }

    public HCCAddDomainResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCAddDomainResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCAddDomainResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion


    public string GetResponseMessage()
    {
      return _message;
    }

    public string GetResponseStatus()
    {
      return _status;
    }

    public int GetResponseStatusCode()
    {
      return _statusCode;
    }
  }
}
