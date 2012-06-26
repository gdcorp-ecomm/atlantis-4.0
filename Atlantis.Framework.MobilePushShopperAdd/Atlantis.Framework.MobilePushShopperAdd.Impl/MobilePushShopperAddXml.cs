using System.Xml.Linq;
using System.Xml.Serialization;

namespace Atlantis.Framework.MobilePushShopperAdd.Impl
{
  public class MobilePushShopperAddXml
  {
    public string RegistrationId { get; set; }

    public string MobileAppId { get; set; }

    public string MobileDeviceId { get; set; }

    public string ShopperId { get; set; }

    public string PushEmail { get; set; }

    [XmlAttribute(AttributeName = "PushEmailSubscriptionID")]
    public long PushEmailSubscriptionId { get; set; }

    public string ToXml()
    {
      XElement mpnInsertElement = new XElement("MPNInsert");

      XAttribute registrationIdAttribute = new XAttribute("RegistrationID", RegistrationId);
      mpnInsertElement.Add(registrationIdAttribute);

      XAttribute mobileAppIdAttribute = new XAttribute("ShopperMobileAppID", MobileAppId);
      mpnInsertElement.Add(mobileAppIdAttribute);
      
      XAttribute mobileDeviceIdAttribute = new XAttribute("DeviceID", MobileDeviceId);
      mpnInsertElement.Add(mobileDeviceIdAttribute);
      
      if(!string.IsNullOrEmpty(ShopperId))
      {
        XAttribute shopperIdAttribute = new XAttribute("ShopperID", ShopperId);
        mpnInsertElement.Add(shopperIdAttribute);
      }

      if (!string.IsNullOrEmpty(PushEmail))
      {
        XAttribute pushEmailAttribute = new XAttribute("PushEmail", PushEmail);
        mpnInsertElement.Add(pushEmailAttribute);
      }

      if (PushEmailSubscriptionId > 0)
      {
        XAttribute pushEmailSubscriptionIdAttribute = new XAttribute("PushEmailSubscriptionID", PushEmailSubscriptionId);
        mpnInsertElement.Add(pushEmailSubscriptionIdAttribute);
      }

      return mpnInsertElement.ToString();
    }
  }
}
