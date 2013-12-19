using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using Atlantis.Framework.Iris.Interface;

namespace Atlantis.Framework.Iris.Impl
{
  public class CreateIncidentRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as CreateIncidentRequestData;
      IResponseData responseData = null;
      IrisWebService irisWebService = null;

      try
      {
        irisWebService = ServiceHelpers.GetServiceReference(((WsConfigElement)config).WSURL);
        irisWebService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        var responseCode = irisWebService.CreateIncidentInIRIS(
                                  request.SubscriberId,
                                  request.Subject,
                                  request.Note,
                                  request.CustomerEmailAddress,
                                  request.IpAddress,
                                  request.GroupId,
                                  request.ServiceId,
                                  request.PrivateLableId,
                                  request.ShopperId,
                                  request.CreatedBy);


        responseData = CreateIncidentResponseData.FromData(responseCode);
      }
      catch (Exception ex)
      {
        responseData = new CreateIncidentResponseData(request, ex);
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
