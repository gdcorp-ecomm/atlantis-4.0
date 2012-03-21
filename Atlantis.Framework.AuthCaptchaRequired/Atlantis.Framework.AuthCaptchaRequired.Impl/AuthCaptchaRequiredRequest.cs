using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthCaptchaRequired.Interface;
using Atlantis.Framework.AuthCaptchaRequired.Impl.AuthWS;
using Atlantis.Framework.ServiceHelper;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.AuthCaptchaRequired.Impl
{
  public class AuthCaptchaRequiredRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthCaptchaRequiredResponseData responseData;

      try
      {
        string wsUrl = ((WsConfigElement)config).WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthCaptchaRequired::RequestHandler", "AuthCaptchaRequired WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          var request = (AuthCaptchaRequiredRequestData)requestData;

          authService.Url = wsUrl;
          authService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          X509Certificate2 clientCert = ClientCertHelper.GetClientCertificate(config);
          if (clientCert == null)
          { 
            throw new AtlantisException(requestData, "AuthCaptchaRequired::RequestHandler", "Unable to find client cert for web service call", string.Empty);
          }
          authService.ClientCertificates.Add(clientCert);

          var retVal = (int)authService.CaptchaRequired(request.IPAddress);
          var isRequired = retVal == 1;

          responseData = new AuthCaptchaRequiredResponseData(isRequired);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthCaptchaRequired::RequestHandler", message, string.Empty);
        responseData = new AuthCaptchaRequiredResponseData(aex);
      }

      return responseData;
    }
  }
}
