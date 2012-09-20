using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetBasketObjects.Interface;
using Atlantis.Framework.GetBasketPrice.Interface;
using Atlantis.Framework.GetBasketPrice.Impl;

namespace Atlantis.Framework.GetBasketObjects.Impl
{
  public class GetBasketObjectsRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GetBasketObjectsResponseData responseData = null;
      try
      {
        string cartXML = GetCartXml(requestData,config);
        responseData = new GetBasketObjectsResponseData(cartXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetBasketObjectsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetBasketObjectsResponseData(requestData, ex);
      }

      return responseData;
    }

    private string GetCartXml(RequestData requestData,ConfigElement config)
    {
      string _cartXML = string.Empty;

      GetBasketPriceRequest newRequest = new GetBasketPriceRequest();
      
      GetBasketPriceRequestData request = new GetBasketPriceRequestData(
      requestData.ShopperID, requestData.SourceURL, requestData.OrderID, requestData.Pathway, requestData.PageCount, true, string.Empty);
      request.RequestTimeout = requestData.RequestTimeout;
      request.BasketType = "gdshop";
      GetBasketPriceResponseData response = newRequest.RequestHandler(request, config) as GetBasketPriceResponseData;
      _cartXML = response.ToXML();
      return _cartXML;
    }
  }


}
