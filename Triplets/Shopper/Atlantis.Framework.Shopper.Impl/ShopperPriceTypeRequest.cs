using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Impl.gdBillingDataService;
using Atlantis.Framework.Shopper.Interface;
using System;

namespace Atlantis.Framework.Shopper.Impl
{
  public class ShopperPriceTypeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      try
      {
        ShopperPriceTypeRequestData shopperPriceTypeRequestData = (ShopperPriceTypeRequestData)requestData;

        if (shopperPriceTypeRequestData.PrivateLabelId <= 0)
        {
          return ShopperPriceTypeResponseData.Standard;
        }

        if (string.IsNullOrEmpty(shopperPriceTypeRequestData.ShopperID))
        {
          return ShopperPriceTypeResponseData.Standard;
        }

        int priceType = 0;
        string outputError = string.Empty;

        using (var wsBillingData = new WSgdBillingDataService())
        {
          wsBillingData.Url = ((WsConfigElement)config).WSURL;
          wsBillingData.Timeout = (int)shopperPriceTypeRequestData.RequestTimeout.TotalMilliseconds;
          priceType = wsBillingData.GetShopperPriceType(shopperPriceTypeRequestData.ShopperID, shopperPriceTypeRequestData.PrivateLabelId, out outputError);
        }

        if (outputError.IndexOf("ERROR", StringComparison.InvariantCultureIgnoreCase) > 0)
        {
          throw new Exception(outputError);
        }

        return ShopperPriceTypeResponseData.FromRawPriceType(priceType, shopperPriceTypeRequestData.PrivateLabelId);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        var exception = new AtlantisException(requestData, "ShopperPriceTypeRequest.RequestHandler()", message, requestData.ToXML());
        Engine.Engine.LogAtlantisException(exception);
        return ShopperPriceTypeResponseData.Standard;      
      }
    }
  }
}
