
namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public static class AuthResetPasswordStatusCodes
  {
    public const int Success = 1;
    public const int Failure = 0;
    public const int Locked = 3;
    public const int SuccessMixed = 2;
    public const int Error = -1;
    public const int PasswordTooShort = -2;
    public const int PasswordTooLong = -3;
    public const int PasswordHintMatch = -6;
    public const int PasswordRequired = -110;
    public const int PasswordInvalidCharacters = -111;
    public const int ShopperIdRequired = -120;
    public const int HintRequired = -130;
    public const int HintMaxLength = -131;
    public const int HintInvalidCharacters = -132;
    public const int IpAddressRequired = -140;
    public const int IpAddressInvalid = -141;
    public const int AuthTokenRequired = -150;
  }
}
