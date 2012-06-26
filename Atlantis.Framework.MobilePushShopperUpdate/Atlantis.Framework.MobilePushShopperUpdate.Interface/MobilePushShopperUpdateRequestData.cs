using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperUpdate.Interface
{
  public class MobilePushShopperUpdateRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(12);

    public int ShopperPushId { get; private set; }

    public string RegistrationId { get; private set; }

    public string MobileAppId { get; private set; }

    public string MobileDeviceId { get; private set; }

    public string PushEmail { get; private set; }

    public long PushEmailSubscriptionId { get; private set; }

    public MobilePushShopperUpdateRequestData(int shopperPushId, string registrationId, string mobileAppId, string mobileDeviceId, string pushEmail, long pushEmailSubscriptionId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperPushId = shopperPushId;
      RegistrationId = registrationId;
      MobileAppId = mobileAppId;
      MobileDeviceId = mobileDeviceId;
      PushEmail = pushEmail;
      PushEmailSubscriptionId = pushEmailSubscriptionId;

      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperUpdate is not a cacheable request.");
    }
  }
}
