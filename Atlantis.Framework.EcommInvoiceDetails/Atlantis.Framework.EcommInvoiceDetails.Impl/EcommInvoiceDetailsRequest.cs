using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommInvoiceDetails.Interface;
using Atlantis.Framework.EcommInvoiceDetails.Impl.BasketWs;

namespace Atlantis.Framework.EcommInvoiceDetails.Impl
{
  public class EcommInvoiceDetailsRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string xmlError = string.Empty;
      EcommInvoiceDetailsResponseData response = null;

      try
      {
        EcommInvoiceDetailsRequestData request = (EcommInvoiceDetailsRequestData)requestData;

        using (WscgdBasketService basketWs = new WscgdBasketService())
        {
          basketWs.Url = ((WsConfigElement)config).WSURL;
          basketWs.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          string xmlInvoiceDetails = basketWs.GetInvoiceDetail(request.InvoiceId, out xmlError);

          if (string.IsNullOrEmpty(xmlError))
          {
            response = new EcommInvoiceDetailsResponseData(xmlInvoiceDetails.Replace("&gt;", ">").Replace("&lt;", "<"));
          } 
          else
          {
            response = new EcommInvoiceDetailsResponseData(requestData, new Exception(xmlError));
          }
        }
      }
      catch (Exception ex)
      {
        response = new EcommInvoiceDetailsResponseData(requestData, ex);
      }

      return response;
    }
  }
}
