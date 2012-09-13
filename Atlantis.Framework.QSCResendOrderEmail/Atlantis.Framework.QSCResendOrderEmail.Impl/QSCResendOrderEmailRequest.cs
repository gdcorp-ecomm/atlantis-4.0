using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCResendOrderEmail.Interface;

namespace Atlantis.Framework.QSCResendOrderEmail.Impl
{
  public class QSCResendOrderEmailRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCResendOrderEmailResponseData responseData = null;
      QSCResendOrderEmailRequestData request = requestData as QSCResendOrderEmailRequestData;

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
						response = service.resendOrderEmail(request.AccountUid, request.ShopperID, request.InvoiceId, request.EmailToResend, request.PackageIds.ToArray(), request.UnpackedContentIds.ToArray());

            if (response != null)
              responseData = new QSCResendOrderEmailResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCResendOrderEmailResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
