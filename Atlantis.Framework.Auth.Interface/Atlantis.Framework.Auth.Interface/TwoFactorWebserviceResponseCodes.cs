
namespace Atlantis.Framework.Auth.Interface
{
  public static class TwoFactorWebserviceResponseCodes
  {
    public const int Error = 0;
    public const int Success = 1;
    public const int LoginError = -1;
    public const int AuthTokenInvalid = -201;
  }
}
