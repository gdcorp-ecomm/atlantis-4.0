using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionSecurityAuth.Interface
{
  public class OrionSecurityAuthResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    public string AuthToken { get; private set; }
    public bool IsSuccess { get; private set; }

    public OrionSecurityAuthResponseData(string authToken)
    {
      AuthToken = authToken;
      IsSuccess = !string.IsNullOrEmpty(authToken);
    }



    public OrionSecurityAuthResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public OrionSecurityAuthResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "OrionSecurityAuthResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return string.Format("<AuthenticateResponse><AuthenticateResult>{0}</AuthenticateResult></AuthenticateResponse>", AuthToken);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
