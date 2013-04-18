using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionEvent.Interface
{
  public class OrionInsertEventResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string ResponseCode { get; set; }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        int code;
        if (!int.TryParse(ResponseCode, out code))
        {
          _isSuccess = false;
        }
        else if (code.Equals(0))
        {
          _isSuccess = false;
        }
        else
        {
          _isSuccess = true;
        }
        return _isSuccess;
      }
    }

    public OrionInsertEventResponseData(string responseCode)
    {
      ResponseCode = responseCode;
    }

     public OrionInsertEventResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public OrionInsertEventResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "OrionInsertEventResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in OrionInsertEventResponseData");     
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
