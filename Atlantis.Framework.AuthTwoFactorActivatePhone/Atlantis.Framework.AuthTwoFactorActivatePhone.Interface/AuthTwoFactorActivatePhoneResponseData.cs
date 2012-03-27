using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorActivatePhone.Interface
{
  public class AuthTwoFactorActivatePhoneResponseData : IResponseData
  {
    private AtlantisException _aex;

    public long StatusCode { get; private set; }

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthTwoFactorActivatePhoneResponseData(long statusCode, HashSet<int> validationCodes, string statusMessage)
    {
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      StatusCode = statusCode;
    }

    public AuthTwoFactorActivatePhoneResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthTwoFactorActivatePhoneResponseData", ex.Message, string.Empty, ex);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
