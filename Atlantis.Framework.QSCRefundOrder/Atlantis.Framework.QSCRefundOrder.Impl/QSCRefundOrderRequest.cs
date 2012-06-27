using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCRefundOrder.Interface;

namespace Atlantis.Framework.QSCRefundOrder.Impl
{
  public class QSCRefundOrderRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCRefundOrderResponseData responseData = null;
      QSCRefundOrderRequestData request = requestData as QSCRefundOrderRequestData;

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.refundOrder(request.AccountUid, request.InvoiceId);

            if (response != null)
              responseData = new QSCRefundOrderResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCRefundOrderResponseData(request, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }
      return responseData;
    }

    #endregion
  }
}
