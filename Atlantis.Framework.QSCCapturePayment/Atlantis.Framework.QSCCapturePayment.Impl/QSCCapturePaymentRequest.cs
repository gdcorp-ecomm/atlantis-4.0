using System;
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

			Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

			try
			{
				using (service)
				{
					if (request != null)
					{
						service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

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
			finally
			{
				if (service != null)
				{
					service.Dispose();
				}
			}
			return responseData;
		}

		#endregion
	}
}
