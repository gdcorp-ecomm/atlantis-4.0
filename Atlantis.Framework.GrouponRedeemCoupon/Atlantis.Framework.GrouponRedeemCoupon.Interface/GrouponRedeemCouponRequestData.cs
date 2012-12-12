using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GrouponRedeemCoupon.Interface
{
  public class GrouponRedeemCouponRequestData : RequestData
  {
    public GrouponRedeemCouponRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string couponCode) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(shopperId))
      {
        throw new ArgumentException("Must provide a value.", "shopperId");
      }
      if (string.IsNullOrEmpty(couponCode))
      {
        throw new ArgumentException("Must provide a value.", "couponCode");
      }
      CouponCode = couponCode;
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    public string CouponCode { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}