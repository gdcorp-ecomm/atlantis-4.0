using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Google;

namespace Atlantis.Framework.GoogleGetDNSKey.Interface
{
  public class GoogleGetDNSKeyResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string _accessToken = string.Empty;
    private readonly bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DateTime RetrieveDate { get; private set; }

    public GoogleGetDNSKeyResponseData(verification ver, DateTime retrieveDate)
    {
      _success = true;
      Response = ver.token;
      RetrieveDate = retrieveDate;
    }

    public GoogleGetDNSKeyResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GoogleGetDNSKeyResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "GoogleGetDNSKeyResponseData",
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