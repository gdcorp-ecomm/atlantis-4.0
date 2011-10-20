using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RemoveBulkdomain.Interface
{
  public class RemoveBulkDomainResponseData : IResponseData
  {

    string _responseXML = string.Empty;
    AtlantisException _exception;

    public RemoveBulkDomainResponseData(string responseXML)
    {
      _responseXML = responseXML;
      _exception = null;
    }

    public RemoveBulkDomainResponseData(string responseXML, AtlantisException ex)
    {
      _responseXML = responseXML;
      _exception = ex;
    }

    public RemoveBulkDomainResponseData(string responseXML, RequestData requestData, Exception ex)
    {
      _responseXML = responseXML;
      _exception = new AtlantisException(requestData, "RemoveBulkDomainResponseData", ex.Message, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return _responseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) ==-1;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
