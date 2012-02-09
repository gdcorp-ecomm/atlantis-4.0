using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDSubmitTroubleTicket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitTroubleTicket.Impl
{
  public class HDVDSubmitTroubleTicketRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesHostingResponse response = null;
      HDVDSubmitTroubleTicketResponseData responseData = null;
      HDVDSubmitTroubleTicketRequestData request = requestData as HDVDSubmitTroubleTicketRequestData;

      HCCAPIServiceAries service = SerivceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.SubmitTroubleTicket(request.AccountGuid, request.CustFirstName, request.CustLastName,
                                                   request.CustEmail, request.CustPhone, request.TicketTitle,
                                                   request.TicketBody, request.HasBeenRebooted,
                                                   request.GrantSupportAccess);

            if (response != null)
              responseData = new HDVDSubmitTroubleTicketResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDSubmitTroubleTicketResponseData(request, ex);

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
