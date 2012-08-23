using System;
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

			Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

			try
			{
				using (service)
				{
					if (request != null)
					{
						service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

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
