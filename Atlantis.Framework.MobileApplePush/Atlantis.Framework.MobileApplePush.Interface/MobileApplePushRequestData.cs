using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(4);

    public MobileApplePushNotification Notification { get; private set; }

    public MobileApplePushRequestData(string message, string deviceToken, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : this(message, null, deviceToken, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public MobileApplePushRequestData(int? badge, string deviceToken, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : this(null, badge, deviceToken, shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public MobileApplePushRequestData(string message, int? badge, string deviceToken, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Notification = new MobileApplePushNotification(message, badge, deviceToken);
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobileApplePush is not a cacheable request");
    }
  }
}
