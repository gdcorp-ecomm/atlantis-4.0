using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrderSearchParameters.Interface;

namespace Atlantis.Framework.QSCGetOrderSearchParameters.Impl
{
  public class QSCGetOrderSearchParametersRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getOrderSearchParametersResponseDetail response = null;
      QSCGetOrderSearchParametersResponseData responseData = null;
      QSCGetOrderSearchParametersRequestData request = requestData as QSCGetOrderSearchParametersRequestData;

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
						response = service.getOrderSearchParameters(request.AccountUid, request.ShopperID);

            if (response != null)
              responseData = new QSCGetOrderSearchParametersResponseData((response as getOrderSearchParametersResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetOrderSearchParametersResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
