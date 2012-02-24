using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetEmailUsage.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetEmailUsage.Impl
{
  public class HDVDGetEmailUsageRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesEmailUsageResponse response = null;
      HDVDGetEmailUsageResponseData responseData = null;
      HDVDGetEmailUsageRequestData request = requestData as HDVDGetEmailUsageRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.GetEmailUsage(request.AccountGuid);

            if (response != null)
              responseData = new HDVDGetEmailUsageResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDGetEmailUsageResponseData(request, ex);

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
