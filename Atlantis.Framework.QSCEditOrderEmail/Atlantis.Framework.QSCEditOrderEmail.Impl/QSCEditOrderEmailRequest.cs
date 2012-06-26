using System;
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

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.editOrderEmail(request.AccountUid, request.InvoiceId, request.EmailAddress);

            if (response != null)
              responseData = new QSCEditOrderEmailResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCEditOrderEmailResponseData(request, ex);
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
