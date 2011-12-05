using System;
using Atlantis.Framework.GetBasket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasket.Impl
{
  public class GetBasketRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      string responseXML = "";

      WSCgdBasket.WscgdBasketService basketWS = null;
      try
      {
        GetBasketRequestData basketRequestData = (GetBasketRequestData)requestData;
        basketWS = new WSCgdBasket.WscgdBasketService();
        basketWS.Url = ((WsConfigElement)config).WSURL;
        basketWS.Timeout = (int)Math.Truncate(basketRequestData.RequestTimeout.TotalMilliseconds);

        if (!string.IsNullOrEmpty(basketRequestData.BasketType))
        {
          responseXML = basketWS.GetBasketXMLByType(
            basketRequestData.ShopperID,
            (short)(basketRequestData.DeleteRefund ? -1 : 0),
            basketRequestData.BasketType);
        }
        else
        {
          responseXML = basketWS.GetBasketXML(
            basketRequestData.ShopperID,
            (short)(basketRequestData.DeleteRefund ? -1 : 0));
        }

        responseData = new GetBasketResponseData(responseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetBasketResponseData(responseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetBasketResponseData(responseXML, requestData, ex);
      }
      finally
      {
        if (basketWS != null)
        {
          basketWS.Dispose();
        }
      }

      return responseData;
    }



    #endregion


  }
}