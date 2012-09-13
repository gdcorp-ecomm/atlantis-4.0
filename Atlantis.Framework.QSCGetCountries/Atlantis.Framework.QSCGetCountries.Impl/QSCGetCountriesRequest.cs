using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetCountries.Interface;

namespace Atlantis.Framework.QSCGetCountries.Impl
{
	public class QSCGetCountriesRequest : IRequest
	{
		#region Implementation of IRequest

		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			getCountriesResponseDetail response = null;
			QSCGetCountriesResponseData responseData = null;
			QSCGetCountriesRequestData request = requestData as QSCGetCountriesRequestData;

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
						response = service.getSupportedCountries(request.AccountUid, request.ShopperID, request.IncludeRegions, request.IncludeRegionsSpecified, request.SortByCountryCode, request.SortByCountryCodeSpecified, request.OrderSearchFields.ToArray());

						if (response != null)
							responseData = new QSCGetCountriesResponseData((response as getCountriesResponseDetail));

					}
				}
			}
			catch (Exception ex)
			{
				responseData = new QSCGetCountriesResponseData(request, ex);
			}
			return responseData;
		}

		#endregion
	}
}
