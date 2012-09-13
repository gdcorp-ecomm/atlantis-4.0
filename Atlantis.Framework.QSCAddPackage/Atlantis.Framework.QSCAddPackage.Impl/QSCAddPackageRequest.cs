using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCAddPackage.Interface;

namespace Atlantis.Framework.QSCAddPackage.Impl
{
  public class QSCAddPackageRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCAddPackageResponseData responseData = null;
      QSCAddPackageRequestData request = requestData as QSCAddPackageRequestData;

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

            response = service.addPackage(request.AccountUid, request.ShopperID, request.InvoiceId, request.Items.ToArray());

            if (response != null)
              responseData = new QSCAddPackageResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCAddPackageResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
