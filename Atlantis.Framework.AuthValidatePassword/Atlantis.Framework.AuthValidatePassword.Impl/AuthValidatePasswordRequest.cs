using System;
using System.Collections.Generic;
using Atlantis.Framework.AuthValidatePassword.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthValidatePassword.Impl.AuthWS;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthValidatePassword.Impl
{
  public class AuthValidatePasswordRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthValidatePasswordResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string wsUrl = wsConfigElement.WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthValidatePassword::RequestHandler", "AuthValidatePassword WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          var request = (AuthValidatePasswordRequestData)requestData;
          var isValidPassword = false;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          HashSet<int> validationCodes = ValidateRequest(request);
          string errorOutput;

          if (validationCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            authService.Url = wsUrl;
            authService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "AuthValidatePassword::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = authService.ValidatePassword(request.ShopperID, request.Password, out errorOutput);
            isValidPassword = statusCode == TwoFactorWebserviceResponseCodes.Success;
          }

          responseData = new AuthValidatePasswordResponseData(isValidPassword, validationCodes, statusCode, errorOutput);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthValidatePassword::RequestHandler", message, string.Empty);
        responseData = new AuthValidatePasswordResponseData(aex);
      }

      return responseData;
    }


    private static HashSet<int> ValidateRequest(AuthValidatePasswordRequestData request)
    {
      HashSet<int> result = new HashSet<int>();    

      #region Password
      if (string.IsNullOrEmpty(request.Password))
      {
        result.Add(AuthValidationCodes.ValidatePasswordRequired);
      }
      #endregion

      return result;
    }
  }
}
