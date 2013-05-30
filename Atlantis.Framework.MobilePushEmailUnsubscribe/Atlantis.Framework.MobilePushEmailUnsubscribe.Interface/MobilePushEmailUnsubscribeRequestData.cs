using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Interface
{
  public class MobilePushEmailUnsubscribeRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    public string Email { get; private set; }

    public long SubscriptionId { get; private set; }

    public bool? IsMobile { get; set; }

    public MobilePushEmailUnsubscribeRequestData(string email, long subscriptionId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      SubscriptionId = subscriptionId;

      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushEmailUnsubscribe is not a cacheable request.");
    }
  }
}
