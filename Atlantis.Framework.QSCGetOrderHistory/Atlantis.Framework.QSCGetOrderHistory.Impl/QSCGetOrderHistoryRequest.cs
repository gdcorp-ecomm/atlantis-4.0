using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrderHistory.Interface;

namespace Atlantis.Framework.QSCGetOrderHistory.Impl
{
	public class QSCGetOrderHistoryRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			getOrderHistoryResponseDetail response = null;
			QSCGetOrderHistoryResponseData responseData = null;
			QSCGetOrderHistoryRequestData request = requestData as QSCGetOrderHistoryRequestData;

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
						response = service.getOrderHistory(request.AccountUid, request.ShopperID, request.InvoiceId);

						if (response != null)
							responseData = new QSCGetOrderHistoryResponseData((response as getOrderHistoryResponseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCGetOrderHistoryResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
