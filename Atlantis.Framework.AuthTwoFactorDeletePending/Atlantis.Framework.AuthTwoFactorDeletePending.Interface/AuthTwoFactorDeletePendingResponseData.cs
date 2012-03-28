using System;
using System.Collections.Generic;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Interface
{
  public class AuthTwoFactorDeletePendingResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public long StatusCode { get; private set; }
    public string StatusMessage { get; private set; }
    public HashSet<int> ValidationCodes { get; private set; }

    public AuthTwoFactorDeletePendingResponseData(long statusCode, string statusMessage, HashSet<int> validationCodes)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
    }

    public AuthTwoFactorDeletePendingResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AuthTwoFactorDeletePendingResponseData"
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
