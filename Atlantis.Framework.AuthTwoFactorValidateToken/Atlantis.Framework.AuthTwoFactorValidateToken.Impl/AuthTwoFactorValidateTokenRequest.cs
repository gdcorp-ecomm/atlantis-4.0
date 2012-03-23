using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.AuthTwoFactorValidateToken.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthTwoFactorValidateToken.Impl.AuthWS;
using Atlantis.Framework.ServiceHelper;
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
        string wsUrl = ((WsConfigElement)config).WSURL;
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
          string resultXml = string.Empty;
          string errorOutput = string.Empty;

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
              throw new AtlantisException(requestData, "AuthTwoFactorValidateToken::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            statusCode = (int)authService.ValidateToken(request.ShopperID, request.AuthToken, request.PhoneNumber, request.HostName, request.IPAddress, out errorOutput);
            isValidToken = statusCode == 1;
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
        //TODO: Add the following validation code.
       // result.Add(AuthValidationCodes.ValidateTokenRequired)
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty(request.IPAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }
      #endregion

      #region PhoneNumber
      if (string.IsNullOrEmpty(request.PhoneNumber))
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
