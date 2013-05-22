using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;
    private readonly string _claimsXml;

    public DotTypeClaimsResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
      _claimsXml = responseXml;
      _isSuccess = true;
    }

    public DotTypeClaimsResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DotTypeClaimsResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "DotTypeClaimsResponseData", ex.Message, requestData.ToXML());
    }

    public string ClaimsXml
    {
      get { return _claimsXml; }
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }
  }
}
