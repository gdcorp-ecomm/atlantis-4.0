using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDValidateTroubleTicket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateTroubleTicket.Impl
{
  public class HDVDValidateTroubleTicketRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesValidationResponse response = null;
      HDVDValidateTroubleTicketResponseData responseData = null;
      HDVDValidateTroubleTicketRequestData request = requestData as HDVDValidateTroubleTicketRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.ValidateFormSubmitTroubleTicket(request.FirstName, request.LastName, request.EmailAddress, request.PhoneNumber, request.Summary, request.Details);

            if (response != null)
              responseData = new HDVDValidateTroubleTicketResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDValidateTroubleTicketResponseData(request, ex);

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
