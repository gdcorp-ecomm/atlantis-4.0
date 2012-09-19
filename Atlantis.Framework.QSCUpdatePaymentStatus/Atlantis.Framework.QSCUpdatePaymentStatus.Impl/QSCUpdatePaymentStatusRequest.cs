using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdatePaymentStatus.Interface;

namespace Atlantis.Framework.QSCUpdatePaymentStatus.Impl
{
	public class QSCUpdatePaymentStatusRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			responseDetail response = null;
			QSCUpdatePaymentStatusResponseData responseData = null;
			var request = requestData as QSCUpdatePaymentStatusRequestData;

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

						//response = service.UpdatePaymentStatus(request.AccountUid, request.ShopperID, request.InvoiceId, request.PaymentId, request.PaymentStatus);
						response = new responseDetail();

						if (response != null)
							responseData = new QSCUpdatePaymentStatusResponseData((response as responseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCUpdatePaymentStatusResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
