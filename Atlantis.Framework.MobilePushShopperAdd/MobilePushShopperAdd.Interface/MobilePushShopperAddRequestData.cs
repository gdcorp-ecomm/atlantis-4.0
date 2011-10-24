
using System;
using Atlantis.Framework.Interface;

namespace MobilePushShopperAdd.Interface
{
  public class MobilePushShopperAddRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(12);

    public TimeSpan RequestTimeout { get; set; }

    public string RegistrationId { get; set; }

    public string MobileAppId { get; set; }

    public string MobileDeviceId { get; set; }

    public MobilePushShopperAddRequestData(string registrationId, string mobileAppId, string mobileDeviceId, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _defaultRequestTimeout;
      RegistrationId = registrationId;
      MobileAppId = mobileAppId;
      MobileDeviceId = mobileDeviceId;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobilePushShopperAdd is not a cacheable request.");
    }
  }
}
