using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Google;

namespace Atlantis.Framework.GoogleGetToken.Interface
{
  public class GoogleGetTokenResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string _accessToken = string.Empty;
    private readonly bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DateTime RetrieveDate { get; private set; }

    public GoogleGetTokenResponseData(auth Auth, DateTime retrieveDate)
    {
      _success = true;
      Response = Auth.access_token;
      RetrieveDate = retrieveDate;
    }

    public GoogleGetTokenResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GoogleGetTokenResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "GoogleGetTokenResponseData",
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