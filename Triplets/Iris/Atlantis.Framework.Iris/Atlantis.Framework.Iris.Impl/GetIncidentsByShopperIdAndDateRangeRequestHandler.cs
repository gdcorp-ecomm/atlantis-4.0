using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using Atlantis.Framework.Iris.Interface;

namespace Atlantis.Framework.Iris.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.Iris.Impl.GetIncidentsByShopperIdAndDateRangeRequestHandler" assembly="Atlantis.Framework.Iris.Impl.dll" request_type="###" />

  public class GetIncidentsByShopperIdAndDateRangeRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as GetIncidentsByShopperIdAndDateRangeRequestData;
      IResponseData responseData = null;
      IrisWebService irisWebService = null;

      try
      {
        irisWebService = ServiceHelpers.GetServiceReference(((WsConfigElement)config).WSURL);
        irisWebService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        var responseXml = irisWebService.GetIncidentsByShopperIdAndDateRange(request.ShopperID, request.StartDate, request.EndDate);

        responseData = GetIncidentsByShopperIdAndDateRangeResponseData.FromData(responseXml);
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
