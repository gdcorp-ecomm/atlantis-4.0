using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdateOrderStatus.Interface;

namespace Atlantis.Framework.QSCUpdateOrderStatus.Impl
{
  public class QSCUpdateOrderStatusRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCUpdateOrderStatusResponseData responseData = null;
      QSCUpdateOrderStatusRequestData request = requestData as QSCUpdateOrderStatusRequestData;

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.updateOrderStatus(request.AccountUid, request.ShopperID, request.InvoiceId, request.OrderStatus);

            if (response != null)
              responseData = new QSCUpdateOrderStatusResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCUpdateOrderStatusResponseData(request, ex);
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
