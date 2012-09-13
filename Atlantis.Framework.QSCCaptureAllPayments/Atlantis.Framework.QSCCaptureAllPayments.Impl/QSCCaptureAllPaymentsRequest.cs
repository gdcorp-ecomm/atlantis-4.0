using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCCaptureAllPayments.Interface;

namespace Atlantis.Framework.QSCCaptureAllPayments.Impl
{
	public class QSCCaptureAllPaymentsRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			responseDetail response = null;
			QSCCaptureAllPaymentsResponseData responseData = null;
			QSCCaptureAllPaymentsRequestData request = requestData as QSCCaptureAllPaymentsRequestData;

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

						response = service.captureAllPayments(request.AccountUid, request.ShopperID, request.InvoiceId);

						if (response != null)
							responseData = new QSCCaptureAllPaymentsResponseData((response as responseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCCaptureAllPaymentsResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
