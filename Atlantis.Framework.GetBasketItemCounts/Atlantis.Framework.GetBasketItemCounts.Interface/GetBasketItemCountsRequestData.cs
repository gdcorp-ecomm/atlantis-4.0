using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketItemCounts.Interface
{
  public class GetBasketItemCountsRequestData : RequestData
  {
    public GetBasketItemCountsRequestData(string shopperId,
                            string sourceUrl,
                            string orderId,
                            string pathway,
                            int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("GetBasketItemCounts is not a cacheable request.");
    }
  }
}
