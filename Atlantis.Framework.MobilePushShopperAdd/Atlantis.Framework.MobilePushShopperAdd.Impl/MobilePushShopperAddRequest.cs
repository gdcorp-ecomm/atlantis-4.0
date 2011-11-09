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
        shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperAddRequestData.RequestTimeout);

        string responseXml = shopperMobilePushService.PushNotificationInsert(mobilePushShopperAddRequestData.RegistrationId,
                                                                             mobilePushShopperAddRequestData.MobileAppId,
                                                                             mobilePushShopperAddRequestData.MobileDeviceId,
                                                                             mobilePushShopperAddRequestData.ShopperID);

        responseData = new MobilePushShopperAddResponseData(mobilePushShopperAddRequestData, responseXml);
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
