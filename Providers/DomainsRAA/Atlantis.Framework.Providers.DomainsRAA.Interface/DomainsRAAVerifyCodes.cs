namespace Atlantis.Framework.Providers.DomainsRAA.Interface
{
  public enum DomainsRAAVerifyCode
  {
    None = -1,

    ShopperArtifactVerified = 100,
    ShopperArtifactVerifyPending = 101,
    ShopperArtifactVerifySuspended = 102,
    ShopperArtifactIsNotVerified = 103,

    DomainRecordVerified = 200,
    DomainRecordNotVerified = 201,
    DomainRecordPendingManualVerify = 202,
    DomainRecordVerifySuspended = 203,
    DomainRecordNotEligibleForRAA = 204,

    ShopperVerified = 300,
    ShopperVerifyPending = 301,
    ShopperVerifySuspended = 302
  }
}
