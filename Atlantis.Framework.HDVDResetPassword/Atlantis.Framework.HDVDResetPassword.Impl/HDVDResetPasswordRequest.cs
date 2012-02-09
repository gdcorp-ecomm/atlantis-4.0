using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDResetPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDResetPassword.Impl
{
  public class HDVDResetPasswordRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesHostingResponse response = null;
      HDVDResetPasswordResponseData responseData = null;
      HDVDResetPasswordRequestData request = requestData as HDVDResetPasswordRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.ResetPassword(request.AccountGuid, request.NewPassword);

            if (response != null)
              responseData = new HDVDResetPasswordResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDResetPasswordResponseData(request, ex);

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
