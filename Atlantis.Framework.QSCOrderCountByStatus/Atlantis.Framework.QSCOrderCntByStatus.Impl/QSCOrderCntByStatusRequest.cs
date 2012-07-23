using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCOrderCntByStatus.Interface;

namespace Atlantis.Framework.QSCOrderCntByStatus.Impl
{
  public class QSCOrderCntByStatusRequest: IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getOrderCountByStatusResponseDetail response = null;
      QSCOrderCntByStatusResponseData responseData = null;
      QSCOrderCntByStatusRequestData request = requestData as QSCOrderCntByStatusRequestData;

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.getOrderCountByStatus(request.AccountUid, request.OnlyDashboardStatusTypes);

            if (response != null)
              responseData = new QSCOrderCntByStatusResponseData((response as getOrderCountByStatusResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCOrderCntByStatusResponseData(request, ex);
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
