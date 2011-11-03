using System;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDS.Interface
{

  public class CDSResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private string _responseData = string.Empty;
    private bool _success = false;
    private HttpStatusCode _statusCode = 0;

    public CDSResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CDSResponseData(string responseData, HttpStatusCode statusCode)
    {
      _success = (statusCode == HttpStatusCode.OK);
      _responseData = responseData;
      _statusCode = statusCode;
    }

    public CDSResponseData(RequestData requestData, HttpStatusCode statusCode, Exception exception)
    {
      _statusCode = statusCode;
      _exception = new AtlantisException(requestData, "CDSResponseData", exception.Message + exception.StackTrace, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public string ResponseData
    {
      get
      {
        return _responseData;
      }
    }

    public HttpStatusCode StatusCode
    {
      get
      {
        return _statusCode;
      }
    }    

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }

}
