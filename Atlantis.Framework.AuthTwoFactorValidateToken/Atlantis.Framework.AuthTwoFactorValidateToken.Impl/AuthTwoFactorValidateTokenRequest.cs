using System;
using System.Collections.Generic;
using Atlantis.Framework.AuthTwoFactorValidateToken.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthTwoFactorValidateToken.Impl.AuthWS;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorValidateToken.Impl
{
  public class AuthTwoFactorValidateTokenRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorValidateTokenResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string wsUrl = wsConfigElement.WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorValidateToken::RequestHandler", "AuthTwoFactorValidateToken WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          var request = (AuthTwoFactorValidateTokenRequestData)requestData;
          var isValidToken = false;
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
              throw new AtlantisException(requestData, "AuthTwoFactorValidateToken::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = authService.ValidateToken(request.ShopperID, request.AuthToken, request.FullPhoneNumber, request.HostName, request.IPAddress, out errorOutput);
            isValidToken = statusCode == TwoFactorWebserviceResponseCodes.Success;
          }

          responseData = new AuthTwoFactorValidateTokenResponseData(isValidToken, validationCodes, statusCode, errorOutput);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthTwoFactorValidateToken::RequestHandler", message, string.Empty);
        responseData = new AuthTwoFactorValidateTokenResponseData(aex);
      }

      return responseData;
    }


    private static HashSet<int> ValidateRequest(AuthTwoFactorValidateTokenRequestData request)
    {
      HashSet<int> result = new HashSet<int>();
      #region AuthToken
      if (string.IsNullOrEmpty(request.AuthToken))
      {
        result.Add(AuthValidationCodes.ValidateAuthTokenRequired);
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty(request.IPAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }
      #endregion

      #region PhoneNumber
      if (string.IsNullOrEmpty(request.FullPhoneNumber))
      {
        result.Add(AuthValidationCodes.ValidatePhoneRequired);
      }
      #endregion

      #region ShopperId
      if (string.IsNullOrEmpty(request.ShopperID))
      {
        result.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }
      #endregion

      return result;
    }
  }
}
