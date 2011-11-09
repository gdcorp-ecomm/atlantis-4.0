using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushShopper.Impl;
using Atlantis.Framework.MobilePushShopperDelete.Interface;

namespace Atlantis.Framework.MobilePushShopperDelete.Impl
{
  public class MobilePushShopperDeleteRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobilePushShopperDeleteRequestData mobilePushShopperDeleteRequestData = (MobilePushShopperDeleteRequestData)requestData;
      MobilePushShopper.Impl.ShopperMobilePushService.Service1 shopperMobilePushService = null;

      try
      {
        shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperDeleteRequestData.RequestTimeout);

        string responseXml = shopperMobilePushService.PushNotificationDelete(mobilePushShopperDeleteRequestData.ShopperPushId);

        responseData = new MobilePushShopperDeleteResponseData(mobilePushShopperDeleteRequestData, responseXml);
      }
      catch (Exception ex)
      {
        responseData = new MobilePushShopperDeleteResponseData(mobilePushShopperDeleteRequestData, ex);
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
