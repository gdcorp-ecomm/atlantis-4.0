using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Interface
{
  public class MobilePushEmailSubscribeRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    public string Email { get; private set; }

    public string PushRegistrationId { get; private set; }

    public MobilePushEmailSubscribeRequestData(string email, string pushRegistrationId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      PushRegistrationId = pushRegistrationId;

      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushEmailSubscribe is not a cacheable request.");
    }
  }
}
