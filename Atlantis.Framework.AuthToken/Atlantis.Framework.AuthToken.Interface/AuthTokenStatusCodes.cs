
namespace Atlantis.Framework.AuthToken.Interface
{
  public static class AuthTokenStatusCodes
  {
    public const int Success = 1;
    public const int Failure = 0;
    public const int Locked = 3;
    public const int SuccessMixed = 2;
    public const int Error = -1;
    public const int ShopperIdRequired = -120;
  }
}
