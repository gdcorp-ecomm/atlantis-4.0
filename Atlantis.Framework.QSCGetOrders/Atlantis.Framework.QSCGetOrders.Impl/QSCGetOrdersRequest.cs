using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrders.Interface;

namespace Atlantis.Framework.QSCGetOrders.Impl
{
  public class QSCGetOrdersRequest :IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getOrdersResponseDetail response = null;
      QSCGetOrdersResponseData responseData = null;
      QSCGetOrdersRequestData request = requestData as QSCGetOrdersRequestData;

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.getOrders(request.AccountUid, request.PageNumber, request.PageSize, request.OrderSearchFields.ToArray());

            if (response != null)
              responseData = new QSCGetOrdersResponseData((response as getOrdersResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetOrdersResponseData(request, ex);
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
