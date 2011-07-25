using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperPriceType.Interface;

namespace Atlantis.Framework.ShopperPriceType.Impl
{
  public class ShopperPriceTypeRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      int priceType = 0;
      string outputError = string.Empty;

      try
      {
        ShopperPriceTypeRequestData shopperPriceTypeRequestData = (ShopperPriceTypeRequestData)requestData;

        using (WSgdBillingData.WSgdBillingDataService wsBillingData = new Atlantis.Framework.ShopperPriceType.Impl.WSgdBillingData.WSgdBillingDataService())
        {
          wsBillingData.Url = ((WsConfigElement)config).WSURL;
          wsBillingData.Timeout = (int)shopperPriceTypeRequestData.RequestTimeout.TotalMilliseconds;
          priceType = wsBillingData.GetShopperPriceType(shopperPriceTypeRequestData.ShopperID, shopperPriceTypeRequestData.PrivateLabelID, out outputError);
        }

        if (outputError.IndexOf("ERROR", StringComparison.InvariantCultureIgnoreCase) > 0)
        {
          throw new AtlantisException(requestData,
                                      "ShopperPriceTypeRequest.RequestHandler()",
                                      outputError,
                                      requestData.ToXML());
        }

        responseData = new ShopperPriceTypeResponseData(priceType, shopperPriceTypeRequestData.PrivateLabelID);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new ShopperPriceTypeResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new ShopperPriceTypeResponseData(requestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
