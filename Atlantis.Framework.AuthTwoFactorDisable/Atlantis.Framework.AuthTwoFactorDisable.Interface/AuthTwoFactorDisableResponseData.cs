﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDisable.Interface
{
  public class AuthTwoFactorDisableResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public HashSet<int> ValidationCodes { get; private set; }

    public long StatusCode { get; private set; }

    public string StatusMessage { get; private set; }


    public AuthTwoFactorDisableResponseData(long statusCode, string statusMessage, HashSet<int> validationCodes)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
    }

    public AuthTwoFactorDisableResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AuthTwoFactorDisableResponseData"
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
