using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Google;

namespace Atlantis.Framework.GoogleDNSInsert.Interface
{
  public class GoogleDNSInsertResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string _accessToken = string.Empty;
    private readonly bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public bool DNSInserted
    {
      get; private set;
    }

    public DateTime RetrieveDate { get; private set; }

    public GoogleDNSInsertResponseData(string response, bool dnsInserted, DateTime retrieveDate)
    {
      _success = true;
      Response = response;
      RetrieveDate = retrieveDate;
      DNSInserted = dnsInserted;
    }

    public GoogleDNSInsertResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GoogleDNSInsertResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "GoogleDNSInsertResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }

    public string Response { get; private set; }

    #region IResponseData Members

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_accessToken))
      {
        if (Response != null)
        {
          _accessToken = Response.ToString();
        }
      }

      return _accessToken;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}