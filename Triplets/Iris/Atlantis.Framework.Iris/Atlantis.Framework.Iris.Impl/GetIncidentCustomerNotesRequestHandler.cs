using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using Atlantis.Framework.Iris.Interface;

namespace Atlantis.Framework.Iris.Impl
{
  public class GetIncidentCustomerNotesRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as GetIncidentCustomerNotesRequestData;
      IResponseData responseData = null;
      IrisWebService irisWebService = null;

      try
      {
        irisWebService = ServiceHelpers.GetServiceReference(((WsConfigElement)config).WSURL);
        irisWebService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        var responseXml = irisWebService.GetIncidentCustomerNotes(request.IncidentId, request.NoteId);

        responseData = GetIncidentCustomerNotesResponseData.FromData(responseXml);
      }
      catch (Exception ex)
      {
        responseData = new GetIncidentCustomerNotesResponseData(request, ex);
      }
      finally
      {
        if (irisWebService != null)
        {
          irisWebService.Dispose();
        }
      }

      return responseData;
    }
  }
}
