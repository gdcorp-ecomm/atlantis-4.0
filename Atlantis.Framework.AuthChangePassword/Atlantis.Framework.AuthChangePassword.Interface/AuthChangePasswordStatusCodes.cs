
namespace Atlantis.Framework.AuthChangePassword.Interface
{
  public class AuthChangePasswordStatusCodes
  {
    public const int Success = 1;
    public const int Failure = 0;
    public const int Locked = 3;
    public const int SuccessMixed = 2;
    public const int Error = -1;
    public const int PasswordToShort = -2;
    public const int PasswordToLong = -3;
    public const int LoginHintMatch = -4;
    public const int LoginPasswordMatch = -5;
    public const int PasswordHintMatch = -6;
    public const int LoginCannotBeNumeric = -7;
    public const int LoginAlreadyTaken = -8;
    public const int PasswordStrengthWeak = -10;
    public const int PasswordStrengthDuplicate = -11;
    public const int PasswordStrengthNoNumeric = -12;
    public const int PasswordStrengthNoCapital = -13;
    public const int PasswordStrengthAlreadyUsed = -14;
    public const int ValidateCurrentPasswordRequired = -100;
    public const int ValidateCurrentPasswordToShort = -101;
    public const int ValidatePasswordRequired = -110;
    public const int ValidatePasswordInvalidCharacters = -111;
    public const int ValidateLoginRequired = -120;
    public const int ValidateLoginMaxLength = -121;
    public const int ValidateLoginInvalidCharacters = -122;
    public const int ValidateHintRequired = -130;
    public const int ValidateHintMaxLength = -131;
    public const int ValidateHintInvalidCharacters = -132;
  }
}
