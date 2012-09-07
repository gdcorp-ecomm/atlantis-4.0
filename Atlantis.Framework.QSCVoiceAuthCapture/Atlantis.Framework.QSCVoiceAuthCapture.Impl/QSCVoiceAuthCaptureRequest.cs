using System;
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

			Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

			try
			{
				using (service)
				{
					if (request != null)
					{
						service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

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
