using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommDelayedPayment.Interface;

namespace Atlantis.Framework.EcommDelayedPayment.Impl
{
  public class EcommDelayedPaymentRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommDelayedPaymentResponseData responseData = null;
      string responseText = string.Empty;
      try
      {
        EcommDelayedPaymentRequestData ecomRequest = (EcommDelayedPaymentRequestData)requestData;
        BasketService.WscgdBasketService oSvc = new BasketService.WscgdBasketService();
        oSvc.Url = ((WsConfigElement)config).WSURL;
        oSvc.Timeout = (int)ecomRequest.RequestTimeout.TotalMilliseconds;
        string invoiceID = string.Empty;
        string redirectDataXML = string.Empty;
        string errorXML = string.Empty;
        oSvc.RequestDelayedPurchase(ecomRequest.ToXML(), out invoiceID, out redirectDataXML, out errorXML);
        System.Diagnostics.Debug.WriteLine(errorXML);
        System.Diagnostics.Debug.WriteLine(redirectDataXML);
        responseData = new EcommDelayedPaymentResponseData(requestData,redirectDataXML, errorXML, invoiceID);       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommDelayedPaymentResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommDelayedPaymentResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
