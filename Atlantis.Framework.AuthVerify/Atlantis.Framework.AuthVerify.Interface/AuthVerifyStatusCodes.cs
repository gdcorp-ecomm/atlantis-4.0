﻿
namespace Atlantis.Framework.AuthVerify.Interface
{
  public static class AuthVerifyStatusCodes
  {
    public const int Success = 1;
    public const int SuccessTwoFactorEnabled = 6;
    public const int Failure = 0;
    public const int Locked = 3;
    public const int SuccessMixed = 2;
    public const int Error = -1;
    public const int ValidatePasswordRequired = -110;
    public const int ValidateLoginNameRequired = -120;
    public const int ValidateIpAddressRequired = -140;
  }
}
