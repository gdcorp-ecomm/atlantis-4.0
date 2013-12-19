using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using Atlantis.Framework.Iris.Interface;
using System;

namespace Atlantis.Framework.Iris.Impl
{
  public class QuickCreateIncidentRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as QuickCreateIncidentRequestData;
      IResponseData responseData = null;
      IrisWebService irisWebService = null;

      try
      {
        irisWebService = ServiceHelpers.GetServiceReference(((WsConfigElement)config).WSURL);
        irisWebService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        var responseCode = irisWebService.CreateQuickIncidentinIRIS(
                                  request.SubscriberId,
                                  request.Subject,
                                  request.Note,
                                  request.CustomerEmailAddress,
                                  request.IpAddress,
                                  request.CreatedBy,
                                  request.PrivateLableId,
                                  request.ShopperId);


        responseData = QuickCreateIncidentResponseData.FromData(responseCode);
      }
      catch (Exception ex)
      {
        responseData = new QuickCreateIncidentResponseData(request, ex);
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
