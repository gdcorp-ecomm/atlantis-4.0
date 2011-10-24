using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperDelete.Interface
{
  public class MobilePushShopperDeleteRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(12);

    public int ShopperPushId { get; set; }

    public MobilePushShopperDeleteRequestData(int shopperPushId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperPushId = shopperPushId;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperDelete is not a cacheable request.");
    }
  }
}
