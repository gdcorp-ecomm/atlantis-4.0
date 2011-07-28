
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
    public const int CurrentPasswordRequired = -100;
    public const int CurrentPasswordToShort = -101;
    public const int PasswordRequired = -110;
    public const int PasswordInvalidCharacters = -111;
    public const int LoginRequired = -120;
    public const int LoginMaxLength = -121;
    public const int LoginInvalidCharacters = -122;
    public const int HintRequired = -130;
    public const int HintMaxLength = -131;
    public const int HintInvalidCharacters = -132;
  }
}
