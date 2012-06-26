using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushShopper.Impl;
using Atlantis.Framework.MobilePushShopperAdd.Interface;

namespace Atlantis.Framework.MobilePushShopperAdd.Impl
{
  public class MobilePushShopperAddRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobilePushShopperAddRequestData mobilePushShopperAddRequestData = (MobilePushShopperAddRequestData) requestData;
      MobilePushShopper.Impl.ShopperMobilePushService.Service1 shopperMobilePushService = null;

      try
      {
        if (!string.IsNullOrEmpty(mobilePushShopperAddRequestData.RegistrationId) &&
            !string.IsNullOrEmpty(mobilePushShopperAddRequestData.MobileAppId))
        {
          shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperAddRequestData.RequestTimeout);

          MobilePushShopperAddXml mobilePushShopperAddXml = new MobilePushShopperAddXml { RegistrationId = mobilePushShopperAddRequestData.RegistrationId,
                                                                                          MobileAppId = mobilePushShopperAddRequestData.MobileAppId,
                                                                                          MobileDeviceId = mobilePushShopperAddRequestData.MobileDeviceId,
                                                                                          ShopperId = mobilePushShopperAddRequestData.ShopperID,
                                                                                          PushEmail = mobilePushShopperAddRequestData.PushEmail,
                                                                                          PushEmailSubscriptionId = mobilePushShopperAddRequestData.PushEmailSubscriptionId };

          string responseXml = shopperMobilePushService.PushNotificationInsertEx(mobilePushShopperAddXml.ToXml());

          responseData = new MobilePushShopperAddResponseData(mobilePushShopperAddRequestData, responseXml);
        }
        else
        {
          throw new Exception("RegistrationId and MobileAppId are required");
        }
      }
      catch (Exception ex)
      {
        responseData = new MobilePushShopperAddResponseData(mobilePushShopperAddRequestData, ex);
      }
      finally
      {
        if(shopperMobilePushService != null)
        {
          shopperMobilePushService.Dispose();
        }
      }

      return responseData;
    }
  }
}
