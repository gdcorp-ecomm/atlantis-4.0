using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OAuth.Interface;
using Atlantis.Framework.OAuth.Interface.Errors;
using Atlantis.Framework.OAuthValidateClientId.Interface;

namespace Atlantis.Framework.OAuthValidateClientId.Impl
{
  public class OAuthValidateClientIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OAuthValidateClientIdResponseData responseData;

      try
      {
        string clientPalmsId = string.Empty;
        string errorCode;
        bool isValidClientId = false;
        int palmsReturnValue = -1;

        var oauthClientRequest = (OAuthValidateClientIdRequestData)requestData;
        if (IsRequestValid(oauthClientRequest, out errorCode))
        {
          using (var palmsWs = new PalmsWs.Service())
          {
            WsConfigElement wsConfigElement = (WsConfigElement)config;
            palmsWs.Url = wsConfigElement.WSURL;
            palmsWs.Timeout = (int)oauthClientRequest.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();
            if (clientCert == null)
            {
              throw new AtlantisException(requestData, "OAuthValidateClientIdRequestData::RequestHandler", "Unable to find client cert for web service call", string.Empty);
            }
            palmsWs.ClientCertificates.Add(clientCert);

            int failedAuthCount;
            string password;
            string lastAuthSuccess;
            string lastAuthAttempt;

            palmsReturnValue = palmsWs.GetLoginByUserName(
              oauthClientRequest.ApplicationId, oauthClientRequest.ClientId, oauthClientRequest.Host, oauthClientRequest.IpAddress, out clientPalmsId,
              out password, out failedAuthCount, out lastAuthSuccess, out lastAuthAttempt
            );

            isValidClientId = palmsReturnValue == PalmsStatusCodes.Success;

            if (!isValidClientId)
            {
              errorCode = TransformPalmsErrorToOAuthError(palmsReturnValue);
            }
          }
        }

        responseData = new OAuthValidateClientIdResponseData(isValidClientId, clientPalmsId, errorCode, palmsReturnValue);
      }
      catch (AtlantisException aex)
      {
        responseData = new OAuthValidateClientIdResponseData(requestData, aex, AuthTokenResponseErrorCodes.InternalServerError);
      }
      catch (Exception ex)
      {
        responseData = new OAuthValidateClientIdResponseData(requestData, ex, AuthTokenResponseErrorCodes.InternalServerError);
      }

      return responseData;
    }

    private bool IsRequestValid(OAuthValidateClientIdRequestData requestData, out string errorCode)
    {
      errorCode = string.Empty;

      if (string.IsNullOrEmpty(requestData.ApplicationId) ||
        string.IsNullOrEmpty(requestData.IpAddress))
      {
        errorCode = AuthTokenResponseErrorCodes.InternalServerError;
      }
      else if (string.IsNullOrEmpty(requestData.ClientId))
      {
        errorCode = AuthTokenResponseErrorCodes.InvalidRequest;
      }

      return string.IsNullOrEmpty(errorCode);
    }

    private string TransformPalmsErrorToOAuthError(int palmsReturnValue)
    {
      string errorCode;

      switch (palmsReturnValue)
      {
        case PalmsStatusCodes.AppDisabled:
        case PalmsStatusCodes.LoginLocked:
          errorCode = AuthTokenResponseErrorCodes.AccessDenied;
          break;
        case PalmsStatusCodes.AppDoesNotExist:
        case PalmsStatusCodes.LoginDoesNotExist:
          errorCode = AuthTokenResponseErrorCodes.Unauthorized;
          break;
        default:
          errorCode = AuthTokenResponseErrorCodes.InternalServerError;
          break;
      }

      return errorCode;
    }
  }
}
