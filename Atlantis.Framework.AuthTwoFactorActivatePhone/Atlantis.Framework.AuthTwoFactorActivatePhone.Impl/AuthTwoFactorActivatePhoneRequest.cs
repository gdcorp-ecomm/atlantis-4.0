using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorActivatePhone.Impl.AuthService;
using Atlantis.Framework.AuthTwoFactorActivatePhone.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorActivatePhone.Impl
{
  public class AuthTwoFactorActivatePhoneRequest : IRequest
  {
    private static HashSet<int> ValidateRequest(AuthTwoFactorActivatePhoneRequestData request)
    {
      HashSet<int> result = new HashSet<int>();
      
      if (string.IsNullOrEmpty(request.AuthToken))
      {
        result.Add(AuthValidationCodes.ValidateAuthTokenRequired);
      }
      
      if (string.IsNullOrEmpty(request.IpAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }

      if (string.IsNullOrEmpty(request.HostName))
      {
        result.Add(AuthValidationCodes.ValidateHostNameRequired);
      }

      if (string.IsNullOrEmpty(request.PhoneNumber))
      {
        result.Add(AuthValidationCodes.ValidatePhoneRequired);
      }

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        result.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      return result;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorActivatePhoneResponseData responseData;

      try
      {
        string wsUrl = ((WsConfigElement)config).WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorActivatePhone::RequestHandler", "AuthTwoFactorActivatePhone WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          AuthTwoFactorActivatePhoneRequestData request = (AuthTwoFactorActivatePhoneRequestData)requestData;
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

            X509Certificate2 clientCert = ClientCertHelper.GetClientCertificate(config);
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "AuthTwoFactorActivatePhone::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = authService.ActivatePhone(request.ShopperID, request.AuthToken, request.PhoneNumber, request.HostName, request.IpAddress, out errorOutput);
          }

          responseData = new AuthTwoFactorActivatePhoneResponseData(statusCode, validationCodes, errorOutput);
        }
      }
      catch (Exception ex)
      {
        responseData = new AuthTwoFactorActivatePhoneResponseData(ex, requestData);
      }

      return responseData;
    }
  }
}
