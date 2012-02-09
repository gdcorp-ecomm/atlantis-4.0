using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDSubmitSyncPasswords.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitSyncPasswords.Impl
{
  public class HDVDSubmitSyncPasswordsRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesHostingResponse response = null;
      HDVDSubmitSyncPasswordsResponseData responseData = null;
      HDVDSubmitSyncPasswordsRequestData request = requestData as HDVDSubmitSyncPasswordsRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.SyncPasswords(request.AccountGuid, request.RootPassword, request.UserName, request.UserPassword, request.FirewallPassword);

            if (response != null)
              responseData = new HDVDSubmitSyncPasswordsResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDSubmitSyncPasswordsResponseData(request, ex);

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
