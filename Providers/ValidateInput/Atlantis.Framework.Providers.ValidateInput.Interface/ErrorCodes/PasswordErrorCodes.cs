namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public class PasswordErrorCodes : ErrorCodesBase
  {
    public static readonly int PasswordLength = 2;
    public static readonly int PasswordEmpty = 5;
    public static readonly int PasswordsNotEqual = 6;
    public static readonly int PasswordRegEx = 10;
  }
}
