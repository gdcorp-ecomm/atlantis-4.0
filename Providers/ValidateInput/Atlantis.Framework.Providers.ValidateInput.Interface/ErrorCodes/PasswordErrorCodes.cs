namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public static class PasswordErrorCodes
  {
    public const int UnknownError = -1;
    public const int PasswordsNotEqual = 1;
    public const int PasswordLength = 2;
    public const int EmptyPassword = 3;
    public const int ValidationRulesLoadError = 4;
    public const int PasswordRegEx = 10;
  }
}
