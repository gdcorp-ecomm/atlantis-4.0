using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDValidateUserPass.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateUserPass.Impl
{
  public class HDVDValidateUserPassRequest: IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesValidationResponse response = null;
      HDVDValidateUserPassResponseData responseData = null;
      HDVDValidateUserPassRequestData request = requestData as HDVDValidateUserPassRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.ValidateFormUsernamePassword(request.AccountGuid, request.HostName, request.UserName, request.Password, request.ValidateFtp, request.FtpUserName, request.FtpPassword, request.FirewallPassword);

            if (response != null)
              responseData = new HDVDValidateUserPassResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDValidateUserPassResponseData(request, ex);

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
