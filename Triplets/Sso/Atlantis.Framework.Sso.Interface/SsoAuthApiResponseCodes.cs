
namespace Atlantis.Framework.Sso.Interface
{
  public static class SsoAuthApiResponseCodes
  {
    public const string Success = "1";
    public const string SuccessTwoFactor = "2";

    public const string FailureInvalidAttempt = "-1";
    public const string FailureAccountLocked = "-2";
    public const string FailureInvalidAttemptUseCaptcha = "-3";
    public const string FailureMissingArguments = "-4";
    public const string FailureUnknownRealm = "-5";
  }
}
