
namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public static class AuthResetPasswordStatusCodes
  {
    public const long Failure = 0;
    public const long Success = 1;
    public const long LockedShopper = 3;
    public const long Error = -1;
    public const long PasswordTooShort = -2;
    public const long PasswordTooLong = -3;
    public const long LoginHintMatch = -4;
    public const long LoginPasswordMatch = -5;
    public const long PasswordHintMatch = -6;
    public const long PasswordFailBlacklisted = -101;
    public const long PasswordFailMinLength = -102;
    public const long PasswordFailMaxLength = -103;
    public const long PasswordFailNoCapital = -104;
    public const long PasswordFailNoNumber = -105;
    public const long PasswordFailMatchesHint = -106;
    public const long PasswordFailThirtyDay = -107;
    public const long PasswordFailLastFive = -108;
    public const long TwoFactorValidationFailed = -201;
    public const long TwoFactorTokenRequired = -202;
  }
}
