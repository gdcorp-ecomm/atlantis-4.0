namespace Atlantis.Framework.GrouponRedeemCoupon.Interface
{
  public enum GrouponRedeemStatus
  {
    Success,
    UnknownError,
    UnknownShopper,
    UnknownCode,
    InvalidCodeAlreadyUsed,
    ExpiredCode,
    InactiveCode,
    InvalidCodeTooLongOrEmpty,
    InvalidCodeShopperAlreadyUsedAnother
  }
}
