using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public class AuthResetPasswordResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public HashSet<int> ValidationCodes { get; private set; }

    public long StatusCode { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthResetPasswordResponseData(long statusCode, HashSet<int> validationCodes, string statusMessage)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
    }

    public AuthResetPasswordResponseData(RequestData requestData, Exception ex) : this(AuthResetPasswordStatusCodes.Error, new HashSet<int>(), string.Empty)
    {
      _atlantisException = new AtlantisException(requestData, "AuthResetPasswordResponseData", string.Format("Unhandled exception reseting password.|{0}|{1}", ex.Message, ex.StackTrace), string.Empty, ex);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }
  }
}
