using System;
using Atlantis.Framework.EcommValidPaymentType.Impl.WscgdPayment;
using Atlantis.Framework.EcommValidPaymentType.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommValidPaymentType.Impl
{
  public class EcommValidPaymentTypeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommValidPaymentTypeRequestData ecommValidPaymentTypeRequestData = (EcommValidPaymentTypeRequestData)requestData;
      EcommValidPaymentTypeResponseData responseData;

      WSCgdPaymentTypesService wscgdPaymentService = null;

      try
      {
        wscgdPaymentService = new WSCgdPaymentTypesService();
        wscgdPaymentService.Url = ((WsConfigElement)config).WSURL;
        wscgdPaymentService.Timeout = (int)ecommValidPaymentTypeRequestData.RequestTimeout.TotalMilliseconds;

        string responseXml;
        int resultCode;

        if (string.IsNullOrEmpty(ecommValidPaymentTypeRequestData.SelectedCountry))
        {
          resultCode = wscgdPaymentService.GetActivePaymentTypesForCurrency(ecommValidPaymentTypeRequestData.BasketType, ecommValidPaymentTypeRequestData.TransactionalCurrencyType, out responseXml);
        }
        else
        {
          resultCode = wscgdPaymentService.GetActivePaymentTypesEx(ecommValidPaymentTypeRequestData.ToXML(), out responseXml);
        }
        responseData = new EcommValidPaymentTypeResponseData(requestData, responseXml, resultCode);
      }
      catch (Exception ex)
      {
        responseData = new EcommValidPaymentTypeResponseData(requestData, ex);
      }
      finally
      {
        if(wscgdPaymentService != null)
        {
          wscgdPaymentService.Dispose();
        }
      }
      return responseData;
    }
  }
}