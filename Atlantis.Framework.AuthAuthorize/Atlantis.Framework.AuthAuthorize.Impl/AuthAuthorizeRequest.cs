using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthAuthorize.Impl.AuthenticationWS;
using Atlantis.Framework.AuthAuthorize.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthAuthorize.Impl
{
  public class AuthAuthorizeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthAuthorizeResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)config).WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "AuthAuthorize.RequestHandler", "AuthAuthorize WS URL in atlantis.config must use https.", string.Empty);
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          int statusCode = AuthAuthorizeStatusCodes.Error;

          AuthAuthorizeRequestData request = (AuthAuthorizeRequestData)requestData;

          HashSet<int> validationCodes = ValidateRequest(request);
          string resultXml = string.Empty;
          string errorOutput;

          if (validationCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            authenticationService.Url = authServiceUrl;
            authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCertificate = ClientCertHelper.GetClientCertificate(config);
            if(clientCertificate == null)
            {
              throw new AtlantisException(requestData, "AuthAuthorize.RequestHandler", "Unable to find client certificate for web service call.", string.Empty);
            }

            authenticationService.ClientCertificates.Add(clientCertificate);

            statusCode = authenticationService.Authorize(request.LoginName, request.Password, request.PrivateLabelId, request.IpAddress, out resultXml, out errorOutput);
          }

          responseData = new AuthAuthorizeResponseData(resultXml, statusCode, validationCodes, errorOutput);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "AuthAuthorizeRequest.RequestHandler", message, string.Empty);
        responseData = new AuthAuthorizeResponseData(aex);
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest(AuthAuthorizeRequestData request)
    {
      HashSet<int> result = new HashSet<int>();

      #region LoginName
      if (string.IsNullOrEmpty(request.LoginName))
      {
        result.Add(AuthValidationCodes.ValidateLoginNameRequired);
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty(request.IpAddress))
      {
        result.Add(AuthValidationCodes.ValidateIpAddressRequired);
      }
      #endregion

      #region Password
      if (string.IsNullOrEmpty(request.Password))
      {
        result.Add(AuthValidationCodes.ValidatePasswordRequired);
      }
      #endregion

      return result;
    }
  }
}
