namespace Atlantis.Framework.Providers.DomainsRAA.Interface
{
  public enum Errors
  {
    Exception = -1,
    FailedToAddMessage = 1,
    InvalidXml = 2,
    ExceptionInRequestProcessing = 3,
    InvalidOrMissingShopperId = 4,
    InvalidOrMissingReasonCode = 5,
    InvalidOrMissingRequestBy = 6,
    InvalidOrMissingItemType = 7,
    InvalidOrMissingDomainId = 8,
    InvalidOrMissingContactId = 9,
    InvalidOrMissingVerifyItemType = 10,
    InvalidOrMissingSourceIp = 11,
    InvalidOrMissingVerifiedIp = 12,
    DatabaseLookupFailed = 13,
    InvalidOrMissingState = 14,
    InvalidOrMissingLastUserNote = 15,
    ErrorDecryptingVerficationToken = 16
  }
}
