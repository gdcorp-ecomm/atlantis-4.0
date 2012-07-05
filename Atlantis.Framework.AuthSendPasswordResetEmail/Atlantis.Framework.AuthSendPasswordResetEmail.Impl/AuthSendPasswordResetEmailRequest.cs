using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.AuthSendPasswordResetEmail.Impl.AuthWS;
using Atlantis.Framework.AuthSendPasswordResetEmail.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthSendPasswordResetEmail.Impl
{
  public class AuthSendPasswordResetEmailRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthSendPasswordResetEmailResponseData responseData;

      try
      {
        var request = (AuthSendPasswordResetEmailRequestData)requestData;

        HashSet<int> validationCodes = ValidateRequest(request);
        string errorOutput = string.Empty;
        long statusCode = AuthResetPasswordStatusCodes.Error;
        string resultXml = string.Empty;

        if (validationCodes.Count > 0)
        {
          errorOutput = "Request not valid.";
        }
        else
        {
          WsConfigElement wsConfigElement = (WsConfigElement)config;
          string wsUrl = wsConfigElement.WSURL;
          if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
          {
            throw new AtlantisException(requestData, "AuthSendPasswordResetEmail::RequestHandler", "AuthSendPasswordResetEmail WS URL in atlantis.config must use https.", string.Empty);
          }

          using (Authentication authService = new Authentication())
          {
            authService.Url = wsUrl;
            authService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "AuthSendPasswordResetEmail::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            authService.ClientCertificates.Add(clientCert);

            if (string.IsNullOrEmpty(request.LocalizationCode))
            {
              resultXml = authService.SendPasswordResetEmail(request.ShopperID, request.PrivateLabelID, out errorOutput);
            }
            else
            {
              resultXml = authService.SendPasswordResetEmailIntl(request.ShopperID, request.PrivateLabelID, request.LocalizationCode, out errorOutput);
            }

            if (resultXml.ToLower().Contains("success"))
            {
              statusCode = AuthResetPasswordStatusCodes.Success;
            }
          }
        }

        responseData = new AuthSendPasswordResetEmailResponseData(statusCode, validationCodes, errorOutput, resultXml);
      }
      catch (AtlantisException aex)
      {
        responseData = new AuthSendPasswordResetEmailResponseData(aex);
      }
      catch (Exception ex)
      {
        responseData = new AuthSendPasswordResetEmailResponseData(ex, requestData);
      }

      return responseData;
    }

    #region Validate
    private static HashSet<int> ValidateRequest(AuthSendPasswordResetEmailRequestData request)
    {
      HashSet<int> validationErrors = new HashSet<int>();

      #region ShopperID
      if (string.IsNullOrEmpty(request.ShopperID))
      {
        validationErrors.Add(AuthValidationCodes.ValidateShopperIdRequired);
      }
      #endregion

      #region PrivateLabelID
      if (request.PrivateLabelID <= 0)
      {
        validationErrors.Add(AuthValidationCodes.ValidatePrivateLabelIdRequired);
      }
      #endregion

      return validationErrors;
    }
    #endregion
  }
}
