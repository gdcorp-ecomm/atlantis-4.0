using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OAuth.Interface.Errors;

namespace Atlantis.Framework.OAuthGetAuthenticationCode.Interface
{
  public class OAuthGetAuthenticationCodeResponseData: IResponseData
  {
    private readonly AtlantisException _aex;

    public string AuthenticationCode { get; private set; }
    public string ErrorCode { get; private set;  }
    public string ErrorDescription { get; set; }
    public bool IsSuccess
    {
      get { return _aex == null && string.IsNullOrEmpty(ErrorCode); }
    }

    #region Constructors
    public OAuthGetAuthenticationCodeResponseData(string authenticationCode, string errorCode, string errorDescription)
    {
      AuthenticationCode = authenticationCode;
      ErrorCode = errorCode;
      ErrorDescription = errorDescription;
    }

    public OAuthGetAuthenticationCodeResponseData(AtlantisException aex)
    {
      _aex = aex;
      ErrorCode = AuthTokenResponseErrorCodes.InternalServerError;
    }

    public OAuthGetAuthenticationCodeResponseData(RequestData requestData, Exception ex)
    {
      _aex = new AtlantisException(requestData, "OAuthGetAuthenticationCodeResponseData", ex.Message, ex.StackTrace);
      ErrorCode = AuthTokenResponseErrorCodes.InternalServerError;
    }
    #endregion

    #region Member Vars
    public string ToXML()
    {
      return string.Format("<AuthenticationCode>{0}</AuthenticationCode><success>{1}</success>", AuthenticationCode, IsSuccess);
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
    #endregion
  }
}
