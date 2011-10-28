using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(4);

    public MobileApplePushNotification Notification { get; private set; }

    public MobileApplePushRequestData(string deviceToken, string message, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Notification = new MobileApplePushNotification(deviceToken, message);
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobileApplePush is not a cacheable request");
    }
  }
}
