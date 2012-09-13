using System;
using System.Security.Cryptography.X509Certificates;
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
      return responseData;
    }

    #endregion
  }
}
