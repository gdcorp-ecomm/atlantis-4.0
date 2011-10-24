using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetBasket.Interface;

namespace Atlantis.Framework.GetBasket.Impl
{
  public class GetBasketRequest : IRequest
  {
  
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      string responseXML = "";

      try
      {
        GetBasketRequestData basketRequestData = (GetBasketRequestData)requestData;
        WSCgdBasket.WscgdBasketService basketWS = new WSCgdBasket.WscgdBasketService();
        basketWS.Url = ((WsConfigElement)config).WSURL;
        basketWS.Timeout = (int)basketRequestData.RequestTimeout.TotalMilliseconds;

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

      return responseData;
    }

    

    #endregion

    
  }
}