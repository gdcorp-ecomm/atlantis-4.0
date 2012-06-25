using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperGet.Interface
{
  public class MobilePushShopperGetRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    public string Identifier { get; private set; }

    public MobilePushShopperGetType PushGetType { get; private set; }

    public MobilePushShopperGetRequestData(string identifier, MobilePushShopperGetType pushGetType, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Identifier = identifier;
      PushGetType = pushGetType;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperGet is not a cacheable request.");
    }
  }
}
