
namespace Atlantis.Framework.HDVD.Interface.Enums
{
  public enum HDVDStatusCodes
  {
    // finished successfully, no problems.
    OK = 0,
    // Account is in penduseraccount.
    AccountNotSetup = 1,
    // Account doesn't exist in Orion
    AccountNotFound = 2,
    // Account is not dhs or vph
    ProductNotSupported = 3,
    // Username is already in use (FTP accounts only).
    UsernameAlreadyUsed = 4,
    // Code requested an event be inserted; this failed.
    FailedToInsertEvent = 5,
    // Additional IP was requested, but the limit of 3 was reached.
    IPLimitReached = 6,
    // Unexpected / unknown error occurred.
    UnknownError = 7,
    // One of the arguments passed in was out of range.
    ArgumentOutOfRange = 8,
    // One of the arguments passed in was null.
    ArgumentNull = 9,
    // No results were found given the search criteria.
    NoResultsFound = 10,
    // Validation was requested and errors were found.
    ValidationErrors = 11,
    // Product feature, such as FTP account, not found on this acct.
    ProductFeatureNotFound = 12,
    // Account can not be logged into;
    // it's reprovisioning, pending setup, etc.
    AccountPendingAction = 13,
  }
}
