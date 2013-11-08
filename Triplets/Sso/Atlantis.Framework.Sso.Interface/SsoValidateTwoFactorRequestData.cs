using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoValidateTwoFactorRequestData : RequestData
  {
    public string EncryptedToken { get; private set; }
    public string TwoFactorCode { get; private set; }

    public SsoValidateTwoFactorRequestData(string encryptedToken, string twoFactorCode)
    {
      EncryptedToken = encryptedToken;
      TwoFactorCode = twoFactorCode;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }
  }
}
