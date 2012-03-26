namespace Atlantis.Framework.Auth.Interface
{
  public static class AuthPasswordCodes
  {
    public const int PasswordFailBlacklisted = -101;
    public const int PasswordFailMinLength = -102;
    public const int PasswordFailMaxLength = -103;
    public const int PasswordFailNoCapital = -104;
    public const int PasswordFailNoNumber = -105;
    public const int PasswordFailMatchesHint = -106;
    public const int PasswordFailThirtyDay = -107;
    public const int PasswordFailLastFive = -108;         
  }
}
