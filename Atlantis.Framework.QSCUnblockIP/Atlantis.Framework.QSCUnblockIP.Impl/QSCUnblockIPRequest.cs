﻿using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUnblockIP.Interface;

namespace Atlantis.Framework.QSCUnblockIP.Impl
{
	public class QSCUnblockIPRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			responseDetail response = null;
			QSCUnblockIPResponseData responseData = null;
			QSCUnblockIPRequestData request = requestData as QSCUnblockIPRequestData;

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
						response = service.unblockIP(request.AccountUid, request.ShopperID, request.IpAddress);

						if (response != null)
							responseData = new QSCUnblockIPResponseData((response as responseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCUnblockIPResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
