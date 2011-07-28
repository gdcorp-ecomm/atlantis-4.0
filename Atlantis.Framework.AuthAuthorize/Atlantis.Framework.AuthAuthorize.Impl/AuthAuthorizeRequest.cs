using System;
using System.Collections.Generic;
using Atlantis.Framework.AuthAuthorize.Impl.AuthenticationWS;
using Atlantis.Framework.AuthAuthorize.Interface;
using Atlantis.Framework.Interface;

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
          authenticationService.Url = authServiceUrl;

          AuthAuthorizeRequestData request = (AuthAuthorizeRequestData)requestData;
          authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          HashSet<int> responseCodes = ValidateRequest(request);
          string resultXml = string.Empty;
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            int resultCode = authenticationService.Authorize(request.LoginName, request.Password, request.PrivateLabelId, request.IpAddress, out resultXml, out errorOutput);
            responseCodes.Add(resultCode);
          }

          responseData = new AuthAuthorizeResponseData(resultXml, responseCodes, errorOutput);
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
        result.Add(AuthAuthorizeStatusCodes.LoginNameRequired);
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty(request.IpAddress))
      {
        result.Add(AuthAuthorizeStatusCodes.IpAddressRequired);
      }
      #endregion

      #region Password
      if (string.IsNullOrEmpty(request.Password))
      {
        result.Add(AuthAuthorizeStatusCodes.PasswordRequired);
      }
      #endregion

      return result;
    }
  }
}
