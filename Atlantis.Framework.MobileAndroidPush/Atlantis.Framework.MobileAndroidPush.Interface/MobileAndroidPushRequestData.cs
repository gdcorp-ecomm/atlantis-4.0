using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileAndroidPush.Interface
{
  public class MobileAndroidPushRequestData : RequestData
  {
    public string AuthToken { get; private set; }

    public MobileAndroidPushNotification Notification { get; private set; }

    public MobileAndroidPushRequestData(string authToken, string registrationId, string message, string messageNamespace, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AuthToken = authToken;
      Notification = new MobileAndroidPushNotification(registrationId, message, messageNamespace);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobileAndroidPush is not a cachable request.");
    }
  }
}
