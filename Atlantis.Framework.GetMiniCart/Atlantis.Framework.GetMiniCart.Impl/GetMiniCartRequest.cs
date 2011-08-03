using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.GetMiniCart.Interface;

namespace Atlantis.Framework.GetMiniCart.Impl
{
  public class GetMiniCartRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetMiniCartResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var miniCartRequestData = (GetMiniCartRequestData)oRequestData;
        using (var basketWS = new WSCgdBasket.WscgdBasketService())
        {
          basketWS.Url = ((WsConfigElement) oConfig).WSURL;
          basketWS.Timeout = (int) miniCartRequestData.RequestTimeout.TotalMilliseconds;

          if (!string.IsNullOrEmpty(miniCartRequestData.BasketType))
          {
            responseXml = basketWS.GetMiniCartXMLByType(miniCartRequestData.ShopperID, miniCartRequestData.BasketType);
          }
          else
          {
            responseXml = basketWS.GetMiniCartXML(miniCartRequestData.ShopperID);
          }
        }

        responseData = new GetMiniCartResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetMiniCartResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetMiniCartResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    #endregion

  }
}
