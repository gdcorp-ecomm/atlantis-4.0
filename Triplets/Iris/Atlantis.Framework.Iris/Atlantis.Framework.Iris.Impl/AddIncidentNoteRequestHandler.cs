using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using Atlantis.Framework.Iris.Interface;

namespace Atlantis.Framework.Iris.Impl
{
  public class AddIncidentNoteRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as AddIncidentNoteRequestData;
      IResponseData responseData = null;
      IrisWebService irisWebService = null;

      try
      {
        irisWebService = ServiceHelpers.GetServiceReference(((WsConfigElement)config).WSURL);
        irisWebService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        var responseCode = irisWebService.AddIncidentNote(request.IncidentId, request.Note, request.LoginId);


        responseData = AddIncidentNoteResponseData.FromData(responseCode);
      }
      catch (Exception ex)
      {
        responseData = new AddIncidentNoteResponseData(request, ex);
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
