using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdateOrderNotes.Interface;

namespace Atlantis.Framework.QSCUpdateOrderNotes.Impl
{
  public class QSCUpdateOrderNotesRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCUpdateOrderNotesResponseData responseData = null;
      QSCUpdateOrderNotesRequestData request = requestData as QSCUpdateOrderNotesRequestData;

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
						response = service.updateOrderNotes(request.AccountUid, request.ShopperID, request.InvoiceId, request.OrderNotes);

            if (response != null)
              responseData = new QSCUpdateOrderNotesResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCUpdateOrderNotesResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
