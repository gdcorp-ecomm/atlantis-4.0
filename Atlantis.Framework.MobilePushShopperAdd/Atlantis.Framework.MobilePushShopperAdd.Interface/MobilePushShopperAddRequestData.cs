using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperAdd.Interface
{
  public class MobilePushShopperAddRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(12);

    public string RegistrationId { get; private set; }

    public string MobileAppId { get; private set; }

    public string MobileDeviceId { get; private set; }

    public string PushEmail { get; private set; }

    public long PushEmailSubscriptionId { get; private set; }

    public MobilePushShopperAddRequestData(string registrationId, string mobileAppId, string mobileDeviceId, string pushEmail, long pushEmailSubscriptionId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RegistrationId = registrationId;
      MobileAppId = mobileAppId;
      MobileDeviceId = mobileDeviceId;
      PushEmail = pushEmail;
      PushEmailSubscriptionId = pushEmailSubscriptionId;

      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperAdd is not a cacheable request.");
    }
  }
}
