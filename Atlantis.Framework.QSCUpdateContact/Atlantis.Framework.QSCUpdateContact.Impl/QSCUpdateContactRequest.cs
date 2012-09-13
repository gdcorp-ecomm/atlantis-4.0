using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdateContact.Interface;

namespace Atlantis.Framework.QSCUpdateContact.Impl
{
  public class QSCUpdateContactRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCUpdateContactResponseData responseData = null;
      QSCUpdateContactRequestData request = requestData as QSCUpdateContactRequestData;

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
						response = service.updateContact(request.AccountUid, request.ShopperID, request.InvoiceId, request.Contact);

            if (response != null)
              responseData = new QSCUpdateContactResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCUpdateContactResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
