
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
        if(!string.IsNullOrEmpty(mobilePushShopperGetRequestData.RegistrationId))
        {
          responseXml = shopperMobilePushService.PushNotificationGetByRegistrationID(mobilePushShopperGetRequestData.RegistrationId);
        }
        else if(!string.IsNullOrEmpty(mobilePushShopperGetRequestData.ShopperID))
        {
          responseXml = shopperMobilePushService.PushNotificationGetByShopper(mobilePushShopperGetRequestData.ShopperID);
        }
        else
        {
          throw new Exception("You must provide either RegistrationId or ShopperId.");
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
