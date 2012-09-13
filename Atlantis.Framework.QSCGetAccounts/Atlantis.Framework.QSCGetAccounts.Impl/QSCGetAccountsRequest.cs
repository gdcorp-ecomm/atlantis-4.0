using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetAccounts.Interface;

namespace Atlantis.Framework.QSCGetAccounts.Impl
{
  public class QSCGetAccountsRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getAccountResponseDetail response = null;
      QSCGetAccountsResponseData responseData = null;
      QSCGetAccountsRequestData request = requestData as QSCGetAccountsRequestData;

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

	          response = service.getAccounts(request.ShopperID);

            if (response != null)
              responseData = new QSCGetAccountsResponseData((response as getAccountResponseDetail));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetAccountsResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
