
namespace Atlantis.Framework.MobilePushShopperGet.Interface
{
  public class MobilePushShopperRecord
  {
    public int ShopperPushId { get; private set; }

    public string RegistrationId { get; private set; }

    public string MobileAppId { get; private set; }

    public string MobileDeviceId { get; private set; }

    internal MobilePushShopperRecord(int shopperPushId, string registrationId, string mobileAppId, string mobileDeviceId)
    {
      ShopperPushId = shopperPushId;
      RegistrationId = registrationId;
      MobileAppId = mobileAppId;
      MobileDeviceId = mobileDeviceId;
    }
  }
}
