using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AdWordReferenceLink.Interface
{
  public class AdWordReferenceLinkRequestData : RequestData
  {

    public string CouponKey { get; set; }

    public AdWordReferenceLinkRequestData(string shopperId, 
                                            string sourceUrL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount, string couponKey) : base(shopperId, sourceUrL, orderId, pathway, pageCount)
    {
      CouponKey = couponKey;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AdWordReferenceLink is not a cacheable request.");
    }

    #endregion
  }
}
