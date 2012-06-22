using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthResetPassword.Impl.AuthenticationWS;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Impl
{
  public class AuthResetPasswordRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthResetPasswordResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) oConfig;
        string authServiceUrl = wsConfigElement.WSURL;

        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException( oRequestData, "AuthResetPassword.RequestHandler", "AuthResetPassword WS URL in atlantis.config must use https.", string.Empty );
        }

        using (Authentication authenticationService = new Authentication())
        {
          long statusCode = AuthResetPasswordStatusCodes.Error;

          AuthResetPasswordRequestData requestData = (AuthResetPasswordRequestData)oRequestData;
          HashSet<int> validationCodes = ValidateRequest(requestData);
          
          string statusMessage;
          if (validationCodes.Count > 0)
          {
            statusMessage = "Validation errors.";
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
            if (clientCertificate == null)
            {
              throw new AtlantisException(requestData, "AuthResetPassword.RequestHandler", "Unable to find client certificate for web service call.", string.Empty);
            }

            authenticationService.ClientCertificates.Add(clientCertificate);

            if(!string.IsNullOrEmpty(requestData.TwoFactorAuthToken))
            {
              statusCode = authenticationService.ResetPasswordWithToken(requestData.ShopperID, requestData.PrivateLabelId, requestData.IpAddress, requestData.NewPassword, requestData.NewHint, requestData.EmailAuthToken, requestData.TwoFactorAuthToken, string.Empty, requestData.HostName, out statusMessage);  
            }
            else
            {
              statusCode = authenticationService.ResetPassword(requestData.ShopperID, requestData.PrivateLabelId, requestData.IpAddress, requestData.NewPassword, requestData.NewHint, requestData.EmailAuthToken, out statusMessage);  
            }
          }

          responseData = new AuthResetPasswordResponseData(statusCode, validationCodes, statusMessage);
        }
      }
      catch (Exception ex)
      {
        responseData = new AuthResetPasswordResponseData(oRequestData, ex);
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthResetPasswordRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region ShopperId

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        result.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }

      #endregion

      #region IpAddress

      if (string.IsNullOrEmpty(request.IpAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }

      #endregion

      #region NewPassword

      if (string.IsNullOrEmpty(request.NewPassword))
      {
        result.Add(AuthValidationCodes.ValidatePasswordRequired);
      }

      #endregion

      #region NewHint

      if (string.IsNullOrEmpty(request.NewHint))
      {
        result.Add(AuthValidationCodes.ValidateHintRequired);
      }
      else
      {
        if (request.NewHint.Length > 255)
        {
          result.Add(AuthValidationCodes.ValidateHintMaxLength);
        }

        if (Regex.Match( request.NewHint, "[^\x20-\x3b\x3f-\x7e]" ).Success)
        {
          result.Add(AuthValidationCodes.ValidateHintInvalidCharacters);
        }
      }

      #endregion

      #region AuthToken

      if (string.IsNullOrEmpty(request.EmailAuthToken))
      {
        result.Add(AuthValidationCodes.ValidateEmailAuthTokenRequired);
      }

      #endregion

      #region Cross-Field Rules

      if (request.NewHint == request.NewPassword)
      {
        result.Add(AuthValidationCodes.ValidatePasswordHintMatch);
      }

      #endregion

      return result;
    }
  }
}
