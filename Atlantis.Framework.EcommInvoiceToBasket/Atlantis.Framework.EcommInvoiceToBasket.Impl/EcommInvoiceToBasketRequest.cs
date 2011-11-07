using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommInvoiceToBasket.Interface;
using Atlantis.Framework.EcommInvoiceToBasket.Impl.BasketWs;

namespace Atlantis.Framework.EcommInvoiceToBasket.Impl
{
  class EcommInvoiceToBasketRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string xmlError = string.Empty;
      EcommInvoiceToBasketResponseData response = null;
      try
      {
        EcommInvoiceToBasketRequestData request = (EcommInvoiceToBasketRequestData)requestData;

        using (WscgdBasketService basketWs = new WscgdBasketService())
        {
          basketWs.Url = ((WsConfigElement)config).WSURL;
          basketWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          int itemCount = basketWs.TransferInvoiceToBasket(request.UID, request.AlternateShopperId, out xmlError );

          if (string.IsNullOrEmpty(xmlError))
          {
            response = new EcommInvoiceToBasketResponseData(itemCount);
          }
          else
          {
            response = new EcommInvoiceToBasketResponseData(requestData, new Exception(xmlError));
          }
        }
      }
      catch (Exception ex)
      {
        response = new EcommInvoiceToBasketResponseData(requestData, ex);        
      }

      return response;
    }
  }
}
