using System;
using Atlantis.Framework.GetBasketPrice.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketPrice.Impl
{
  class GetBasketPriceAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      GetBasketPriceRequestData oBasketRequestData = (GetBasketPriceRequestData)oRequestData;
      WscgdBasket.WscgdBasketService oBasketWS = new WscgdBasket.WscgdBasketService();
      oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;
      oBasketWS.Timeout = (int)oBasketRequestData.RequestTimeout.TotalMilliseconds;

      oState = oBasketRequestData.BasketType;
      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oBasketWS, oState);

      IAsyncResult result;
      if (!string.IsNullOrEmpty(oBasketRequestData.BasketType))
      {
        result = oBasketWS.BeginGetBasketPriceXMLByType(
          oBasketRequestData.ShopperID,
          oBasketRequestData.PaymentType,
          (short)(oBasketRequestData.DeleteRefund ? -1 : 0),
          oBasketRequestData.BasketType,
          oCallback,
          oAsyncState);
      }
      else
      {
        result = oBasketWS.BeginGetBasketPriceXML(
          oBasketRequestData.ShopperID,
          oBasketRequestData.PaymentType,
          (short)(oBasketRequestData.DeleteRefund ? -1 : 0),
          oCallback,
          oAsyncState);
      }
      return result;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;
      string sResponseXML = "";

      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;
      string basketType = oAsyncState.State.ToString();

      try
      {
        WscgdBasket.WscgdBasketService oBasketWS = (WscgdBasket.WscgdBasketService)oAsyncState.Request;
        if (!string.IsNullOrEmpty(basketType))
        {
          oResponseData = new GetBasketPriceResponseData(oBasketWS.EndGetBasketPriceXMLByType(oAsyncResult));
        }
        else
        {
          oResponseData = new GetBasketPriceResponseData(oBasketWS.EndGetBasketPriceXML(oAsyncResult));
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GetBasketPriceResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new GetBasketPriceResponseData(sResponseXML, oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
