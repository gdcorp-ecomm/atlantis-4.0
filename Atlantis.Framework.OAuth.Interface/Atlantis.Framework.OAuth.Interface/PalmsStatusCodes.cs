namespace Atlantis.Framework.OAuth.Interface
{
  public struct PalmsStatusCodes
  {
    // Success
    public const int Success = 0;

    // Validation
    public const int InputValidationFailed = -100;
    public const int InvalidAppHash = -101;

    // Permissions related
    public const int PrivDisabled = -200;
    public const int PrivDoesNotExist = -201;
    public const int PrivAlreadyExists = -202;
    public const int PrivCertNotRecognized = -203;
    public const int PrivNotAuthorized = -204;
    public const int PrivCanNotModifyCertUsedForCurrentCall = -205;
    public const int PrivCanNotAddUnrecognizedCert = -206;

    // App related
    public const int AppDisabled = -300;
    public const int AppDoesNotExist = -301;
    public const int AppAlreadyExists = -302;
    public const int AppDoesNotEnforceUniqueUserNames = -303;

    // Login related
    public const int LoginLocked = -400;
    public const int LoginDoesNotExist = -401;
  }
}
