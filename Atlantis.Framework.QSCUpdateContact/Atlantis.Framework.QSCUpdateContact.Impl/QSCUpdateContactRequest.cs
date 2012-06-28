using System;
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

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.updateContact(request.AccountUid, request.InvoiceId, request.OrderContact);

            if (response != null)
              responseData = new QSCUpdateContactResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCUpdateContactResponseData(request, ex);
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
