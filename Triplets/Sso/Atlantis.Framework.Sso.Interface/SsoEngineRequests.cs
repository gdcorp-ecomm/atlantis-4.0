
namespace Atlantis.Framework.Sso.Interface
{
  public static class SsoEngineRequests
  {
    public static int SsoValidateShopperAndGetTokenRequest { get; set; }
    public static int SsoValidateTwoFactorRequest { get; set; }
    public static int SsoGetKeyRequest { get; set; }

    public static int SsoPLValidateShopperAndGetTokenRequest { get; set; }
    public static int SsoPLValidateTwoFactorRequest { get; set; }
    public static int SsoPLGetKeyRequest { get; set; }

    public static int SsoDBPValidateShopperAndGetTokenRequest { get; set; }
    public static int SsoDBPValidateTwoFactorRequest { get; set; }
    public static int SsoDBPGetKeyRequest { get; set; }

    static SsoEngineRequests()
    {
      SsoValidateShopperAndGetTokenRequest = 747;
      SsoValidateTwoFactorRequest = 748;
      SsoGetKeyRequest = 749;
      SsoPLValidateShopperAndGetTokenRequest = 750;
      SsoPLValidateTwoFactorRequest = 751;
      SsoPLGetKeyRequest = 752;
      SsoDBPValidateShopperAndGetTokenRequest = 754;
      SsoDBPValidateTwoFactorRequest = 755;
      SsoDBPGetKeyRequest = 756;

    }
  }
}
