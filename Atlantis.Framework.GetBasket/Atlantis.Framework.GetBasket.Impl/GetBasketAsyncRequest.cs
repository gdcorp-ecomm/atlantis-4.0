using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetBasket.Interface;

namespace Atlantis.Framework.GetBasket.Impl
{
  class GetBasketAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      GetBasketRequestData basketRequestData = (GetBasketRequestData)requestData;
      WSCgdBasket.WscgdBasketService basketWS = new WSCgdBasket.WscgdBasketService();
      basketWS.Url = ((WsConfigElement)config).WSURL;
      basketWS.Timeout = (int)basketRequestData.RequestTimeout.TotalMilliseconds;

      state = basketRequestData.BasketType;
      AsyncState asyncState = new AsyncState(requestData, config, basketWS, state);

      IAsyncResult result;
      if (!string.IsNullOrEmpty(basketRequestData.BasketType))
      {
        result = basketWS.BeginGetBasketXMLByType(
          basketRequestData.ShopperID,
          (short)(basketRequestData.DeleteRefund ? -1 : 0),
          basketRequestData.BasketType,
          callback,
          asyncState);
      }
      else
      {
        result = basketWS.BeginGetBasketXML(
          basketRequestData.ShopperID,
          (short)(basketRequestData.DeleteRefund ? -1 : 0),
          callback,
          asyncState);
      }
      return result;
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      IResponseData responseData = null;
      string responseXML = "";

      AsyncState asyncState = (AsyncState)asyncResult.AsyncState;
      string basketType = asyncState.State.ToString();

      try
      {
        WSCgdBasket.WscgdBasketService basketWS = (WSCgdBasket.WscgdBasketService)asyncState.Request;
        if (!string.IsNullOrEmpty(basketType))
        {
          responseData = new GetBasketResponseData(basketWS.EndGetBasketXMLByType(asyncResult));
        }
        else
        {
          responseData = new GetBasketResponseData(basketWS.EndGetBasketXML(asyncResult));
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetBasketResponseData(responseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetBasketResponseData(responseXML, asyncState.RequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
