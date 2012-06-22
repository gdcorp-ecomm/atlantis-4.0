namespace Atlantis.Framework.Auth.Interface
{
  public static class AuthValidationCodes
  {
    public const int ValidateShopperIdRequired = -100;
    public const int ValidatePasswordRequired = -110;
    public const int ValidateLoginNameRequired = -120;
    public const int ValidateHintRequired = -130;
    public const int ValidateHintMaxLength = -131;
    public const int ValidateHintInvalidCharacters = -132;
    public const int ValidatePasswordHintMatch = -133;
    public const int ValidateIpAddressRequired = -140;
    public const int ValidateHostNameRequired = -150;
    public const int ValidatePhoneRequired = -160;
    public const int ValidateCarrierRequired = -170;
    public const int ValidateAuthTokenRequired = -180;
    public const int ValidateEmailAuthTokenRequired = -190;
  }
}
