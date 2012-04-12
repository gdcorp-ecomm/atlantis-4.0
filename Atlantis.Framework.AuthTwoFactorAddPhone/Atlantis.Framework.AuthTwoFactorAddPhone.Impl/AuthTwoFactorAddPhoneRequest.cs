using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorAddPhone.Impl.WsgdAuthentication;
using Atlantis.Framework.AuthTwoFactorAddPhone.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorAddPhone.Impl
{
  public class AuthTwoFactorAddPhoneRequest : IRequest
  {
    private HashSet<int> ValidateRequest(AuthTwoFactorAddPhoneRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if (string.IsNullOrEmpty(request.Phone.PhoneNumber))
      {
        validationCodes.Add(AuthValidationCodes.ValidatePhoneRequired);
      }

      if (string.IsNullOrEmpty(request.Phone.CarrierId))
      {
        validationCodes.Add(AuthValidationCodes.ValidateCarrierRequired);
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
      AuthTwoFactorAddPhoneResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string authServiceUrl = wsConfigElement.WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorAddPhoneRequest.RequestHandler", "AuthTwoFactorAddPhone WS URL in atlantis.config must use https.", string.Empty);
        }

        X509Certificate2 cert = wsConfigElement.GetClientCertificate();
        cert.Verify();

        using (Authentication authenticationService = new Authentication())
        {
          string statusMessage;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          var request = (AuthTwoFactorAddPhoneRequestData)requestData;

          HashSet<int> validationCodes = ValidateRequest(request);

          if (validationCodes.Count > 0)
          {
            statusMessage = "Request failed validation.";
            responseData = new AuthTwoFactorAddPhoneResponseData(statusCode, statusMessage, validationCodes);
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            authenticationService.ClientCertificates.Add(cert);

            statusCode = authenticationService.AddPhone(request.ShopperID, request.Phone.ToXml(), request.HostName, request.IpAddress, out statusMessage);
            responseData = new AuthTwoFactorAddPhoneResponseData(statusCode, statusMessage, validationCodes);

          }
        }
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorAddPhoneResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
