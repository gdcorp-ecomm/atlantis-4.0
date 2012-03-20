
namespace Atlantis.Framework.AuthAuthorize.Interface
{
	public static class AuthAuthorizeStatusCodes
	{
    public const int Error = -1;
    public const int Failure = 0;
    public const int Success = 1;
    public const int SuccessMixed = 2;
    public const int Locked = 3;
    public const int SuccessExpired = 4;
    public const int FraudPasswordCorrect = 5;
    public const int SuccessTwoFactorEnabled = 6;
    public const int SuccessMixedTwoFactorEnabled = 7;
    public const int SuccessSingleUsePasswordConsumed = 8;
	}
}
