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
        shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperUpdateRequestData.RequestTimeout);

        string responseXml = shopperMobilePushService.PushNotificationUpdate(mobilePushShopperUpdateRequestData.ShopperPushId,
                                                                             mobilePushShopperUpdateRequestData.RegistrationId,
                                                                             mobilePushShopperUpdateRequestData.MobileAppId,
                                                                             mobilePushShopperUpdateRequestData.MobileDeviceId,
                                                                             mobilePushShopperUpdateRequestData.ShopperID);

        responseData = new MobilePushShopperUpdateResponseData(mobilePushShopperUpdateRequestData, responseXml);
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
