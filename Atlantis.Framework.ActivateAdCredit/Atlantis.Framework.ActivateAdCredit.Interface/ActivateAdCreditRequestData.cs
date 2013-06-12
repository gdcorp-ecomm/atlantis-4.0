using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ActivateAdCredit.Interface
{
  public class ActivateAdCreditRequestData : RequestData
  {

    public int CouponKey { get; set; }

    public ActivateAdCreditRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount, int couponKey)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CouponKey = couponKey;
    }

  public override string GetCacheMD5()
  {
    throw new Exception("ActivateAdCreditRequestData is not a cachable request");
  }

  }
}
