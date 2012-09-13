using System;
using System.Security.Cryptography.X509Certificates;
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
						response = service.getOrders(request.AccountUid, request.ShopperID, request.PageNumber, request.PageSize, request.OrderSearchFields.ToArray());

            if (response != null)
              responseData = new QSCGetOrdersResponseData((response as getOrdersResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetOrdersResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
