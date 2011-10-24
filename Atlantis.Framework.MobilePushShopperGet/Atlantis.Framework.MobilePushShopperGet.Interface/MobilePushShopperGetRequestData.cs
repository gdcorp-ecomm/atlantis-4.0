using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperGet.Interface
{
  public class MobilePushShopperGetRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(12);

    public string RegistrationId { get; private set; }

    public MobilePushShopperGetRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : this(string.Empty, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public MobilePushShopperGetRequestData(string registrationId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _defaultRequestTimeout;
      RegistrationId = registrationId;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperGet is not a cacheable request.");
    }
  }
}
