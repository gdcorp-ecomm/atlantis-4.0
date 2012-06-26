using System.Xml.Linq;

namespace Atlantis.Framework.MobilePushShopperUpdate.Impl
{
  internal class MobilePushShopperUpdateXml
  {
    public int ShopperPushId { get; set; }

    public string RegistrationId { get; set; }

    public string MobileAppId { get; set; }

    public string MobileDeviceId { get; set; }

    public string ShopperId { get; set; }

    public string PushEmail { get; set; }

    public long PushEmailSubscriptionId { get; set; }

    public string ToXml()
    {
      XElement mpnUpdateElement = new XElement("MPNUpdate");

      XAttribute shopperPushNotificationIdAttribute = new XAttribute("ShopperMobilePushNotificationID", ShopperPushId);
      mpnUpdateElement.Add(shopperPushNotificationIdAttribute);

      XAttribute registrationIdAttribute = new XAttribute("RegistrationID", RegistrationId);
      mpnUpdateElement.Add(registrationIdAttribute);

      XAttribute mobileAppIdAttribute = new XAttribute("ShopperMobileAppID", MobileAppId);
      mpnUpdateElement.Add(mobileAppIdAttribute);

      XAttribute mobileDeviceIdAttribute = new XAttribute("DeviceID", MobileDeviceId);
      mpnUpdateElement.Add(mobileDeviceIdAttribute);

      if (!string.IsNullOrEmpty(ShopperId))
      {
        XAttribute shopperIdAttribute = new XAttribute("ShopperID", ShopperId);
        mpnUpdateElement.Add(shopperIdAttribute);
      }

      if (!string.IsNullOrEmpty(PushEmail))
      {
        XAttribute pushEmailAttribute = new XAttribute("PushEmail", PushEmail);
        mpnUpdateElement.Add(pushEmailAttribute);
      }

      if (PushEmailSubscriptionId > 0)
      {
        XAttribute pushEmailSubscriptionIdAttribute = new XAttribute("PushEmailSubscriptionID", PushEmailSubscriptionId);
        mpnUpdateElement.Add(pushEmailSubscriptionIdAttribute);
      }

      return mpnUpdateElement.ToString();
    }
  }
}
