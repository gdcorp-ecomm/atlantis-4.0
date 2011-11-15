using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileAndroidPush.Interface
{
  public class MobileAndroidPushRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(5);

    public MobileAndroidPushNotification Notification { get; private set; }

    public MobileAndroidPushRequestData(string registrationId, string message, string messageNamespace, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Notification = new MobileAndroidPushNotification(registrationId, message, messageNamespace);
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobileAndroidPush is not a cachable request.");
    }
  }
}
