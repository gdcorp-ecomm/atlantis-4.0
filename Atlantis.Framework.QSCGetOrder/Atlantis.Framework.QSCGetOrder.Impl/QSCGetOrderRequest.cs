using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrder.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;

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

			Mobilev10	service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

			try
			{
				using (service)
				{
					if (request != null)
					{
						service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

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
