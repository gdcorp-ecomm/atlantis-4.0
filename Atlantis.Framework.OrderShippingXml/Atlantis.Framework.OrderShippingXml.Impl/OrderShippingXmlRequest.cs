using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrderShippingXml.Impl.OrderSvc;
using Atlantis.Framework.OrderShippingXml.Interface;

namespace Atlantis.Framework.OrderShippingXml.Impl
{
  public class OrderShippingXmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OrderShippingXmlResponseData responseData = null;

      try
      {
        OrderShippingXmlRequestData orderShippingXmlRequest = (OrderShippingXmlRequestData)requestData;
        int orderId = 0;
        int.TryParse(orderShippingXmlRequest.RecentOrderId, out orderId);

        MarketplaceOrder marketplaceOrder;
        Dictionary<int, Interface.ShippingItem> shippingItemsDictionary = new Dictionary<int, Interface.ShippingItem>(1);

        using(Order orderWS = new Order())
        {
          orderWS.Url = (((WsConfigElement)config).WSURL);
          orderWS.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          marketplaceOrder = orderWS.GetOrderShippingInfo(orderId);
        }


        foreach (OrderSvc.ShippingItem shippingItem in marketplaceOrder.Items)
        {
          Interface.ShippingItem item = new Interface.ShippingItem(shippingItem.RowID, shippingItem.Carrier, shippingItem.TrackingCode, shippingItem.EstDate);
          shippingItemsDictionary.Add(shippingItem.RowID, item);
        }

        responseData = new OrderShippingXmlResponseData(shippingItemsDictionary);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new OrderShippingXmlResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new OrderShippingXmlResponseData(requestData, ex);
      }

      return responseData;
    }
  }

}
