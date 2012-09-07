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

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.getOrderCountByStatus(request.AccountUid, request.ShopperID, request.OnlyDashboardStatusTypes);

            if (response != null)
              responseData = new QSCOrderCntByStatusResponseData((response as getOrderCountByStatusResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCOrderCntByStatusResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
