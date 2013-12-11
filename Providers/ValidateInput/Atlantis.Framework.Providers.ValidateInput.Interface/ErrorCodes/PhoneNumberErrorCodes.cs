namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public static class PhoneNumberErrorCodes
  {
    public const int UnknownError = -1;
    public const int InvalidCountryCode = 1;
    public const int TooShort = 2;
    public const int TooLong = 3;
    public const int InvalidPhoneNumber = 4;
    public const int EmptyPhoneNumber = 5;
  }
}