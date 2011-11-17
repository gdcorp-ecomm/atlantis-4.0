using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionSSOAuthToken.Interface
{
  public class OrionSSOAuthTokenResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    public string SsoAuthToken { get; private set; }

    public OrionSSOAuthTokenResponseData(string ssoAuthToken)
    {
      SsoAuthToken = ssoAuthToken;
    }

     public OrionSSOAuthTokenResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public OrionSSOAuthTokenResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "OrionSSOAuthTokenResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return new XDocument(new XElement("SsoAuthToken", SsoAuthToken)).ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
