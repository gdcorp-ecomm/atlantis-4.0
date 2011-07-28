
namespace Atlantis.Framework.AuthAuthorize.Interface
{
  public static class AuthAuthorizeStatusCodes
  {
    public const int Success = 1;
    public const int Failure = 0;
    public const int Locked = 3;
    public const int SuccessMixed = 2;
    public const int Error = -1;
    public const int PasswordRequired = -110;
    public const int LoginNameRequired = -120;
    public const int IpAddressRequired = -140;
    public const int IpAddressInvalid = -141;
  }
}
