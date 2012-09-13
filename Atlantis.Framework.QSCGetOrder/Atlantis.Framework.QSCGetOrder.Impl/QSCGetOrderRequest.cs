using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrder.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.QSCGetOrder.Impl
{
	public class QSCGetOrderRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			getOrderResponseDetail response = null;
			QSCGetOrderResponseData responseData = null;
			QSCGetOrderRequestData request = requestData as QSCGetOrderRequestData;

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
						response = service.getOrder(request.AccountUid, request.ShopperID, request.InvoiceId);

						if (response != null)
							responseData = new QSCGetOrderResponseData((response as getOrderResponseDetail));
					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCGetOrderResponseData(request, ex);
			}
			return responseData;
		}

		#endregion

	}
}
