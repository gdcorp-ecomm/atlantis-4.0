using System;
using System.Collections.Generic;

using Atlantis.Framework.AuthVerify.Impl.AuthenticationWS;
using Atlantis.Framework.AuthVerify.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthVerify.Impl
{
  public class AuthVerifyRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthVerifyResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase ))
        {
          throw new AtlantisException( oRequestData, "AuthVerify.RequestHandler", "AuthVerify WS URL in atlantis.config must use https.", string.Empty );
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          authenticationService.Url = authServiceUrl;
          authenticationService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

          AuthVerifyRequestData request = (AuthVerifyRequestData)oRequestData;
          HashSet<int> responseCodes = ValidateRequest(request);
          string loginKey = String.Empty;
          string validationSource = String.Empty;
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            int resultCode = authenticationService.Verify(request.LoginName, request.Password, request.PrivateLabelId, request.IpAddress, out loginKey, out validationSource, out errorOutput);
            responseCodes.Add(resultCode);
          }

          responseData = new AuthVerifyResponseData(loginKey, validationSource, responseCodes, errorOutput);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthVerifyResponseData( exAtlantis );
      }
      catch (Exception ex)
      {
        responseData = new AuthVerifyResponseData( oRequestData, ex );
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthVerifyRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region LoginName
      if (string.IsNullOrEmpty( request.LoginName ))
      {
        result.Add(AuthVerifyStatusCodes.ValidateLoginNameRequired);
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty( request.IpAddress ))
      {
        result.Add(AuthVerifyStatusCodes.ValidateIpAddressRequired);
      }
      #endregion

      #region Password
      if (string.IsNullOrEmpty( request.Password ))
      {
        result.Add(AuthVerifyStatusCodes.ValidatePasswordRequired);
      }
      #endregion

      return result;
    }
  }
}
