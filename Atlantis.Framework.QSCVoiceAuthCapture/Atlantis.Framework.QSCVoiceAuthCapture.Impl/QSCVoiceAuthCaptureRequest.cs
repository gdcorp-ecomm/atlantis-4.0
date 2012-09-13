using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCVoiceAuthCapture.Interface;

namespace Atlantis.Framework.QSCVoiceAuthCapture.Impl
{
	public class QSCVoiceAuthCaptureRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			responseDetail response = null;
			QSCVoiceAuthCaptureResponseData responseData = null;
			QSCVoiceAuthCaptureRequestData request = requestData as QSCVoiceAuthCaptureRequestData;

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
						response = service.voiceAuthCapture(request.AccountUid, request.ShopperID, request.InvoiceId, request.PaymentId, request.PaymentIdSpecified, request.VoiceAuthCode);

						if (response != null)
							responseData = new QSCVoiceAuthCaptureResponseData((response as responseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCVoiceAuthCaptureResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
