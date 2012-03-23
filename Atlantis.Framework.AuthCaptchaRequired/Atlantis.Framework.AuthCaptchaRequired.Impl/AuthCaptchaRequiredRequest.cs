using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthCaptchaRequired.Impl.AuthWS;
using Atlantis.Framework.AuthCaptchaRequired.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthCaptchaRequired.Impl
{
  public class AuthCaptchaRequiredRequest : IRequest
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
          var isRequired = false;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          HashSet<int> validationCodes = ValidateRequest(request);
          string resultXml = string.Empty;
          string errorOutput = string.Empty;

          if (validationCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            authService.Url = wsUrl;
            authService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = ClientCertHelper.GetClientCertificate(config);
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "AuthCaptchaRequired::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

           statusCode = authService.CaptchaRequired(request.IPAddress);
           isRequired = statusCode == TwoFactorWebserviceResponseCodes.Success;
          }

          responseData = new AuthCaptchaRequiredResponseData(isRequired, validationCodes, statusCode, errorOutput);
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


    private static HashSet<int> ValidateRequest(AuthCaptchaRequiredRequestData request)
    {
      HashSet<int> result = new HashSet<int>();

      #region IpAddress
      if (string.IsNullOrEmpty(request.IPAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }
      #endregion

      return result;
    }
  }
}
