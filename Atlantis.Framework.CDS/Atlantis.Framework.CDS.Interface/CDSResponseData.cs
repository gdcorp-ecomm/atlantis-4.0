using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CDS.Interface
{
  public class CDSResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly bool _success;
    private readonly string _responseData;

    public CDSResponseData(string responseData)
    {
      _responseData = responseData;
      _success = true;
    }

    [Obsolete("Only exists for backward compatibility.  Use CDSResponseData(string responseData) instead.")]
    public CDSResponseData(string responseData, bool success)
    {
      _responseData = responseData;
      _success = success;
    }

    public CDSResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, exception.Source, exception.Message + exception.StackTrace, requestData.ToXML());
      _success = false;
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

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
