using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCCapturePayment.Interface;

namespace Atlantis.Framework.QSCCapturePayment.Impl
{
	public class QSCCapturePaymentRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			responseDetail response = null;
			QSCCapturePaymentResponseData responseData = null;
			QSCCapturePaymentRequestData request = requestData as QSCCapturePaymentRequestData;

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

						response = service.capturePayment(request.AccountUid, request.ShopperID, request.InvoiceId, request.PaymentId, request.PaymentIdSpecified);

						if (response != null)
							responseData = new QSCCapturePaymentResponseData((response as responseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCCapturePaymentResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
