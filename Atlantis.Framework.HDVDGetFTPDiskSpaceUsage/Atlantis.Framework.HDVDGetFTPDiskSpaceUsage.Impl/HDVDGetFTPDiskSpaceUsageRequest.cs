using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetFTPDiskSpaceUsage.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetFTPDiskSpaceUsage.Impl
{
  public class HDVDGetFTPDiskSpaceUsageRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesFTPUsageResponse response = null;
      HDVDGetFTPDiskSpaceUsageResponseData responseData = null;
      HDVDGetFTPDiskSpaceUsageRequestData request = requestData as HDVDGetFTPDiskSpaceUsageRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.GetFTPDiskSpaceUsage(request.AccountGuid);

            if (response != null)
              responseData = new HDVDGetFTPDiskSpaceUsageResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDGetFTPDiskSpaceUsageResponseData(request, ex);

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
