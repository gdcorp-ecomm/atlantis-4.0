using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorDeletePending.Impl.wsGdAuthentication;
using Atlantis.Framework.AuthTwoFactorDeletePending.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Impl
{
  public class AuthTwoFactorDeletePendingRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorDeletePendingResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string authServiceUrl = wsConfigElement.WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorDeletePendingRequest.RequestHandler", "AuthTwoFactorDeletePending WS URL in atlantis.config must use https.", string.Empty);
        }

        X509Certificate2 cert = wsConfigElement.GetClientCertificate();
        cert.Verify();

        using (Authentication authenticationService = new Authentication())
        {
          string statusMessage;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;          
          
          var request = (AuthTwoFactorDeletePendingRequestData)requestData;

          HashSet<int> validationCodes = ValidateRequest(request);

          if (validationCodes.Count > 0)
          {
            statusMessage = "Request failed validation.";
            responseData = new AuthTwoFactorDeletePendingResponseData(statusCode, statusMessage, validationCodes);
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            authenticationService.ClientCertificates.Add(cert);
            
            statusCode = authenticationService.DeletePendingTwoFactor(request.ShopperID
              , request.PrivateLableId
              , request.HostName
              , request.IpAddress
              , out statusMessage);
            responseData = new AuthTwoFactorDeletePendingResponseData(statusCode, statusMessage, validationCodes);

          }
        }
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorDeletePendingResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Validate Request
    private HashSet<int> ValidateRequest(AuthTwoFactorDeletePendingRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
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
    #endregion

  }
}
