namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public static class PasswordErrorCodes
  {
    public const int UnknownError = ErrorCodesBase.UnknownError;
    public const int NoInputs = ErrorCodesBase.NoInputs;
    public const int PasswordLength = 2;
    public const int InvalidInputType = ErrorCodesBase.InvalidInputType;
    public const int ValidationRulesLoadError = ErrorCodesBase.ValidationRulesLoadError;
    public const int PasswordEmpty = 5;
    public const int PasswordsNotEqual = 6;
    public const int PasswordRegEx = 10;
  }
}
