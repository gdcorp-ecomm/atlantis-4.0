
namespace Atlantis.Framework.MobilePushShopperGet.Interface
{
  public class MobilePushShopperRecord
  {
    public int ShopperPushId { get; private set; }

    public string RegistrationId { get; private set; }

    public string MobileAppId { get; private set; }

    public string MobileDeviceId { get; private set; }

    public string ShopperId { get; set; }

    public string PushEmail { get; set; }

    public int PushEmailSubscriptionId { get; set; }

    internal MobilePushShopperRecord(int shopperPushId, string registrationId, string mobileAppId, string mobileDeviceId, string shopperId,
      string pushEmail, int pushEmailSubscriptionId)
    {
      ShopperPushId = shopperPushId;
      RegistrationId = registrationId;
      MobileAppId = mobileAppId;
      MobileDeviceId = mobileDeviceId;
      ShopperId = shopperId;
      PushEmail = pushEmail;
      PushEmailSubscriptionId = pushEmailSubscriptionId;
    }
  }
}
