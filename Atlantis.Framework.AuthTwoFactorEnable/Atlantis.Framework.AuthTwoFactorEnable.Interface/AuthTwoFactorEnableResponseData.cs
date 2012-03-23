using System.Collections.Generic;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorEnable.Interface
{
  public class AuthTwoFactorEnableResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public HashSet<int> ValidationCodes { get; private set; }

    public long StatusCode { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthTwoFactorEnableResponseData(long statusCode, string statusMessage, HashSet<int> validationCodes)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
    }

    public AuthTwoFactorEnableResponseData(AtlantisException ex)
    {
      _exception = ex;
      ValidationCodes = new HashSet<int>();
      StatusCode = TwoFactorWebserviceResponseCodes.Error;
      StatusMessage = "Unknown error";
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
