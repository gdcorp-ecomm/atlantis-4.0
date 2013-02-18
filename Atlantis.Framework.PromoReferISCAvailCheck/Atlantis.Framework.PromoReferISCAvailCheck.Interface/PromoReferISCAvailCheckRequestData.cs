using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoReferISCAvailCheck.Interface
{
  public class PromoReferISCAvailCheckRequestData : RequestData
  {

    private string couponCode;

    public string CouponCode
    {
      get { return couponCode; }
    }
    
    public PromoReferISCAvailCheckRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string couponCode)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
        RequestTimeout = TimeSpan.FromSeconds(15);
        this.couponCode = couponCode;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in PromoReferISCAvailCheckRequestData");     
    }

  }
}
