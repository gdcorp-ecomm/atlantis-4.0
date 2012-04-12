using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorEnable.Impl.WsGdAuthentication;
using Atlantis.Framework.AuthTwoFactorEnable.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorEnable.Impl
{
  public class AuthTwoFactorEnableRequest : IRequest
  {
    private HashSet<int> ValidateRequest(AuthTwoFactorEnableRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if(string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if(string.IsNullOrEmpty(request.Password))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePasswordRequired);
      }

      if(string.IsNullOrEmpty(request.Phone.PhoneNumber))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePhoneRequired);
      }

      if(string.IsNullOrEmpty(request.Phone.CarrierId))
      {
        validationCodes.Add(AuthValidationCodes.ValidateCarrierRequired);
      }

      if(string.IsNullOrEmpty(request.IpAddress))
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
      AuthTwoFactorEnableResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string authServiceUrl = wsConfigElement.WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorEnableRequest.RequestHandler", "AuthTwoFactorEnable WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authenticationService = new Authentication())
        {
          string statusMessage;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          AuthTwoFactorEnableRequestData request = (AuthTwoFactorEnableRequestData) requestData;

          HashSet<int> validationCodes = ValidateRequest(request);

          if(validationCodes.Count > 0)
          {
            statusMessage = "Request failed validation.";
            responseData = new AuthTwoFactorEnableResponseData(statusCode, statusMessage, validationCodes);
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
            if(clientCertificate == null)
            {
              throw new Exception("Client certificate not found.");
            }

            authenticationService.ClientCertificates.Add(clientCertificate);
            statusCode = authenticationService.EnableTwoFactor(request.ShopperID, request.Password, request.PrivateLableId, request.Phone.ToXml(), request.HostName, request.IpAddress, out statusMessage);
            responseData = new AuthTwoFactorEnableResponseData(statusCode, statusMessage, validationCodes);
          }
        }
      }
      catch(Exception ex)
      {
        string message = ex.Message + " | " + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthTwoFactorEnableRequest.RequestHandler", message, string.Empty);
        responseData = new AuthTwoFactorEnableResponseData(aex);
      }

      return responseData;
    }
  }
}
