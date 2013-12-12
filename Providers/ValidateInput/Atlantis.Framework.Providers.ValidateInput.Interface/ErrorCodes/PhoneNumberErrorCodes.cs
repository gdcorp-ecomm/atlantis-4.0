namespace Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes
{
  public class PhoneNumberErrorCodes : ErrorCodesBase
  {
    public static readonly int PhoneNumberEmpty = 2;
    public static readonly int TooShort = 5;
    public static readonly int TooLong = 6;
    public static readonly int InvalidPhoneNumber = 7;
    public static readonly int InvalidCountryCode = 8;
  }
}