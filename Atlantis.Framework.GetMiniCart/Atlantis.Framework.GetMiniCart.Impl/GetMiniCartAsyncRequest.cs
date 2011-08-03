using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.GetMiniCart.Interface;

namespace Atlantis.Framework.GetMiniCart.Impl
{
  public class GetMiniCartAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      var miniCartRequestData = (GetMiniCartRequestData)oRequestData;
      using (var basketWS = new WSCgdBasket.WscgdBasketService())
      {
        basketWS.Url = ((WsConfigElement) oConfig).WSURL;
        basketWS.Timeout = (int) miniCartRequestData.RequestTimeout.TotalMilliseconds;

        oState = miniCartRequestData.BasketType;
        var oAsyncState = new AsyncState(oRequestData, oConfig, basketWS, oState);

        IAsyncResult result;
        if (!string.IsNullOrEmpty(miniCartRequestData.BasketType))
        {
          result = basketWS.BeginGetMiniCartXMLByType(miniCartRequestData.ShopperID, miniCartRequestData.BasketType,
                                                      oCallback, oAsyncState);
        }
        else
        {
          result = basketWS.BeginGetMiniCartXML(miniCartRequestData.ShopperID, oCallback, oAsyncState);
        }

        return result;
      }
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData responseData;
      string responseXml = string.Empty;

      var oAsyncState = (AsyncState)oAsyncResult.AsyncState;
      string basketType = oAsyncState.State.ToString();

      try
      {
        using (var basketWS = (WSCgdBasket.WscgdBasketService)oAsyncState.Request)
        {
          if (!string.IsNullOrEmpty(basketType))
          {
            responseData = new GetMiniCartResponseData(basketWS.EndGetMiniCartXMLByType(oAsyncResult));
          }
          else
          {
            responseData = new GetMiniCartResponseData(basketWS.EndGetMiniCartXML(oAsyncResult));
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetMiniCartResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetMiniCartResponseData(responseXml, oAsyncState.RequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
