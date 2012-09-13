using System;
using System.Security.Cryptography.X509Certificates;
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

			WsConfigElement wsConfigElement = ((WsConfigElement)config);
			Mobilev10 service = ServiceHelper.GetServiceReference(wsConfigElement.WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

						if (!string.IsNullOrEmpty(wsConfigElement.GetConfigValue("ClientCertificateName")))
						{
							X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
							service.ClientCertificates.Add(clientCertificate);
						}
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
