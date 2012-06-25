using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushShopper.Impl;
using Atlantis.Framework.MobilePushShopperGet.Interface;

namespace Atlantis.Framework.MobilePushShopperGet.Impl
{
  public class MobilePushShopperGetRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobilePushShopperGetRequestData mobilePushShopperGetRequestData = (MobilePushShopperGetRequestData)requestData;
      MobilePushShopper.Impl.ShopperMobilePushService.Service1 shopperMobilePushService = null;

      try
      {
        shopperMobilePushService = ShopperMobilePushServiceClient.GetWebServiceInstance((WsConfigElement)config, mobilePushShopperGetRequestData.RequestTimeout);

        string responseXml;

        switch (mobilePushShopperGetRequestData.PushGetType)
        {
          case MobilePushShopperGetType.Shopper:
            responseXml = shopperMobilePushService.PushNotificationGetByShopper(mobilePushShopperGetRequestData.Identifier);
            break;
          case MobilePushShopperGetType.RegistrationId:
            responseXml = shopperMobilePushService.PushNotificationGetByRegistrationID(mobilePushShopperGetRequestData.Identifier);
            break;
          case MobilePushShopperGetType.Email:
            responseXml = shopperMobilePushService.PushNotificationGetByPushEmail(mobilePushShopperGetRequestData.Identifier);
            break;
          default:
            throw new Exception("Invalid MobilePushShopperGetType.");
        }

        responseData = new MobilePushShopperGetResponseData(mobilePushShopperGetRequestData, responseXml);
      }
      catch (Exception ex)
      {
        responseData = new MobilePushShopperGetResponseData(mobilePushShopperGetRequestData, ex);
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
