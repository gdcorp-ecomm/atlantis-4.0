using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorDisable.Impl.WsGdAuthentication;
using Atlantis.Framework.AuthTwoFactorDisable.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDisable.Impl
{
  public class AuthTwoFactorDisableRequest : IRequest
  {
    private HashSet<int> ValidateRequest(AuthTwoFactorDisableRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if (string.IsNullOrEmpty(request.Password))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePasswordRequired);
      }

      if (string.IsNullOrEmpty(request.AuthToken))
      {
        validationCodes.Add(AuthValidationCodes.ValidateAuthTokenRequired);
      }

      if (string.IsNullOrEmpty(request.FullPhoneNumber))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePhoneRequired);
      }
      
      if (string.IsNullOrEmpty(request.IpAddress))
      {
        validationCodes.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }

      if (string.IsNullOrEmpty(request.HostName))
      {
        validationCodes.Add(AuthValidationCodes.ValidateHostNameRequired);
      }

      return validationCodes;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorDisableResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string authServiceUrl = wsConfigElement.WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorDisableRequest.RequestHandler", "AuthTwoFactorDisable WS URL in atlantis.config must use https.", string.Empty);
        }

        X509Certificate2 cert = wsConfigElement.GetClientCertificate();
        cert.Verify();

        using (Authentication authenticationService = new Authentication())
        {
          string statusMessage;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;          

          var request = (AuthTwoFactorDisableRequestData) requestData;

          HashSet<int> validationCodes = ValidateRequest(request);

          if (validationCodes.Count > 0)
          {
            statusMessage = "Request failed validation.";
            responseData = new AuthTwoFactorDisableResponseData(statusCode, statusMessage, validationCodes);
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            authenticationService.ClientCertificates.Add(cert);

            statusCode = authenticationService.DisableTwoFactor(request.ShopperID
              , request.Password
              , request.PrivateLableId
              , request.AuthToken
              , request.FullPhoneNumber
              , request.HostName
              , request.IpAddress
              , out statusMessage);
            responseData = new AuthTwoFactorDisableResponseData(statusCode, statusMessage, validationCodes);

          }
        }
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorDisableResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
