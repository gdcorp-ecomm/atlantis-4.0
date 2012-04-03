using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorDeletePhone.Impl.WsgdAuthentication;
using Atlantis.Framework.AuthTwoFactorDeletePhone.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Impl
{
  public class AuthTwoFactorDeletePhoneRequest : IRequest
  {
    #region Validate Request
    private HashSet<int> ValidateRequest(AuthTwoFactorDeletePhoneRequestData request)
    {
      HashSet<int> validationCodes = new HashSet<int>();

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationCodes.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      if (string.IsNullOrEmpty(request.PhoneNumber))
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
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorDeletePhoneResponseData responseData = null;

      try
      {
        string authServiceUrl = ((WsConfigElement)config).WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorDeletePhoneRequest.RequestHandler", "AuthTwoFactorDeletePhone WS URL in atlantis.config must use https.", string.Empty);
        }

        X509Certificate2 cert = ClientCertHelper.GetClientCertificate(config);
        cert.Verify();

        using (Authentication authenticationService = new Authentication())
        {
          string statusMessage = string.Empty;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;
          
          var request = (AuthTwoFactorDeletePhoneRequestData)requestData;

          HashSet<int> validationCodes = ValidateRequest(request);

          if (validationCodes.Count > 0)
          {
            statusMessage = "Request failed validation.";
            responseData = new AuthTwoFactorDeletePhoneResponseData(statusCode, statusMessage, validationCodes);
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            authenticationService.ClientCertificates.Add(cert);
            
            statusCode = authenticationService.DeletePhone(request.ShopperID, request.PhoneNumber, request.HostName, request.IpAddress, out statusMessage);
            responseData = new AuthTwoFactorDeletePhoneResponseData(statusCode, statusMessage, validationCodes);

          }
        }
      }

      catch (Exception ex)
      {
        responseData = new AuthTwoFactorDeletePhoneResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
