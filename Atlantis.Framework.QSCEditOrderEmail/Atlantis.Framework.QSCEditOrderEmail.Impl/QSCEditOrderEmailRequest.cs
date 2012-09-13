using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCEditOrderEmail.Interface;

namespace Atlantis.Framework.QSCEditOrderEmail.Impl
{
  public class QSCEditOrderEmailRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCEditOrderEmailResponseData responseData = null;
      QSCEditOrderEmailRequestData request = requestData as QSCEditOrderEmailRequestData;

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

            response = service.editOrderEmail(request.AccountUid, request.ShopperID, request.InvoiceId, request.EmailAddress);

            if (response != null)
              responseData = new QSCEditOrderEmailResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCEditOrderEmailResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
