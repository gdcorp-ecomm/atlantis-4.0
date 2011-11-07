using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommInvoiceCancel.Interface;
using Atlantis.Framework.EcommInvoiceCancel.Impl.BasketWs;

namespace Atlantis.Framework.EcommInvoiceCancel.Impl
{
  class EcommInvoiceCancelRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string xmlError = string.Empty;
      EcommInvoiceCancelResponseData response = null;

      try
      {
        EcommInvoiceCancelRequestData request = (EcommInvoiceCancelRequestData)requestData;

        using (WscgdBasketService basketWs = new WscgdBasketService())
        {
          basketWs.Url = ((WsConfigElement)config).WSURL;
          basketWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          short success = basketWs.CancelInvoice(request.UID, out xmlError);

          if (string.IsNullOrEmpty(xmlError))
          {
            response = new EcommInvoiceCancelResponseData(success);
          }
          else
          {
            response = new EcommInvoiceCancelResponseData(requestData, new Exception(xmlError));
          }          
        }
      }
      catch (Exception ex)
      {
        response = new EcommInvoiceCancelResponseData(requestData, ex);
      }

      return response;
    }
  }
}
