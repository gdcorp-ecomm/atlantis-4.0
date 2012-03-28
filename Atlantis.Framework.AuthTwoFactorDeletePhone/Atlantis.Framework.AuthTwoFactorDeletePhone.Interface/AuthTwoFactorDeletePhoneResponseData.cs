using System;
using System.Collections.Generic;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Interface
{
  public class AuthTwoFactorDeletePhoneResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public HashSet<int> ValidationCodes { get; private set; }

    public long StatusCode { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthTwoFactorDeletePhoneResponseData(long statusCode, string statusMessage, HashSet<int> validationCodes)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
    }



    public AuthTwoFactorDeletePhoneResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AuthTwoFactorDeletePhoneResponseData"
        , exception.Message
        , requestData.ToXML());
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
