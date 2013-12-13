namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public static class PhoneNumberErrorCodes
  {
    public const int UnknownError = ErrorCodesBase.UnknownError;
    public const int NoInputs = ErrorCodesBase.NoInputs;
    public const int PhoneNumberEmpty = 2;
    public const int InvalidInputType = ErrorCodesBase.InvalidInputType;
    public const int ValidationRulesLoadError = ErrorCodesBase.ValidationRulesLoadError;
    public const int TooShort = 5;
    public const int TooLong = 6;
    public const int InvalidPhoneNumber = 7;
    public const int InvalidCountryCode = 8;
  }
}