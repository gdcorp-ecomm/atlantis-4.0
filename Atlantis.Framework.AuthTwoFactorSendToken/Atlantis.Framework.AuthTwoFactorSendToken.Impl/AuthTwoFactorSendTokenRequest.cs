using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorSendToken.Impl.AuthWS;
using Atlantis.Framework.AuthTwoFactorSendToken.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorSendToken.Impl
{
  public class AuthTwoFactorSendTokenRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthTwoFactorSendTokenResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) config;
        string wsUrl = wsConfigElement.WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthTwoFactorSendToken::RequestHandler", "AuthTwoFactorSendToken WS URL in atlantis.config must use https.", string.Empty);
        }

        using (Authentication authService = new Authentication())
        {
          var request = (AuthTwoFactorSendTokenRequestData)requestData;
          var tokenSent = false;
          var statusCode = TwoFactorWebserviceResponseCodes.Error;

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
              throw new AtlantisException(requestData, "AuthTwoFactorSendToken::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = (int)authService.SendToken(request.ShopperID, request.FullPhoneNumber, request.HostName, request.IPAddress, out errorOutput);
            tokenSent = statusCode == TwoFactorWebserviceResponseCodes.Success;
          }

          responseData = new AuthTwoFactorSendTokenResponseData(tokenSent, validationCodes, statusCode, errorOutput);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthTwoFactorSendToken::RequestHandler", message, string.Empty);
        responseData = new AuthTwoFactorSendTokenResponseData(aex);
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest(AuthTwoFactorSendTokenRequestData request)
    {
      HashSet<int> result = new HashSet<int>();

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

      #region HostName
      if (string.IsNullOrEmpty(request.HostName))
      {
        result.Add(AuthValidationCodes.ValidateHostNameRequired);
      }
      #endregion

      return result;
    }
  }
}
