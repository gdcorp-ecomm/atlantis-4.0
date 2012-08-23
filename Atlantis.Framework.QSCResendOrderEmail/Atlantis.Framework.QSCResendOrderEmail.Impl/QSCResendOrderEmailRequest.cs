using System;
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

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

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
