using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushShopper.Impl;
using Atlantis.Framework.MobilePushShopperUpdate.Interface;

namespace Atlantis.Framework.MobilePushShopperUpdate.Impl
{
  public class MobilePushShopperUpdateRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobilePushShopperUpdateRequestData mobilePushShopperUpdateRequestData = (MobilePushShopperUpdateRequestData)requestData;
      MobilePushShopper.Impl.ShopperMobilePushService.Service1 shopperMobilePushService = null;
      try
      {
        if (mobilePushShopperUpdateRequestData.ShopperPushId > 0 &&
           !string.IsNullOrEmpty(mobilePushShopperUpdateRequestData.RegistrationId))
        {

          shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperUpdateRequestData.RequestTimeout);


          MobilePushShopperUpdateXml mobilePushShopperUpdateXml = new MobilePushShopperUpdateXml { ShopperPushId = mobilePushShopperUpdateRequestData.ShopperPushId,
                                                                                                   RegistrationId = mobilePushShopperUpdateRequestData.RegistrationId,
                                                                                                   MobileAppId = mobilePushShopperUpdateRequestData.MobileAppId,
                                                                                                   MobileDeviceId = mobilePushShopperUpdateRequestData.MobileDeviceId,
                                                                                                   ShopperId = mobilePushShopperUpdateRequestData.ShopperID,
                                                                                                   PushEmail = mobilePushShopperUpdateRequestData.PushEmail,
                                                                                                   PushEmailSubscriptionId = mobilePushShopperUpdateRequestData.PushEmailSubscriptionId };

          string responseXml = shopperMobilePushService.PushNotificationUpdateEx(mobilePushShopperUpdateXml.ToXml());

          responseData = new MobilePushShopperUpdateResponseData(mobilePushShopperUpdateRequestData, responseXml);
        }
        else
        {
          throw new Exception("ShopperPushId and RegistrationId are required");
        }
      }
      catch (Exception ex)
      {
        responseData = new MobilePushShopperUpdateResponseData(mobilePushShopperUpdateRequestData, ex);
      }
      finally
      {
        if (shopperMobilePushService != null)
        {
          shopperMobilePushService.Dispose();
        }
      }

      return responseData;
    }
  }
}
