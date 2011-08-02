
namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public static class AuthResetPasswordStatusCodes
  {
    public const int Failure = 0;
    public const int Success = 1;
    public const int Error = -1;
    public const int PasswordTooShort = -2;
    public const int PasswordTooLong = -3;
    public const int LoginHintMatch = -4;
    public const int LoginPasswordMatch = -5;
    public const int PasswordHintMatch = -6;
    public const int PasswordHasNoNumeric = -12;
    public const int PasswordHasNoCapitals = -13;
    public const int PasswordPreviouslyUsed = -14;

    public const int ValidatePasswordRequired = -110;
    public const int ValidatePasswordInvalidCharacters = -111;
    public const int ValidateShopperIdRequired = -120;
    public const int ValidateHintRequired = -130;
    public const int ValidateHintMaxLength = -131;
    public const int ValidateHintInvalidCharacters = -132;
    public const int ValidateIpAddressRequired = -140;
    public const int ValidateAuthTokenRequired = -150;
  }
}
