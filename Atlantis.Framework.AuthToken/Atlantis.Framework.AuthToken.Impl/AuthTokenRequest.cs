using System;
using System.Collections.Generic;

using Atlantis.Framework.AuthToken.Impl.AuthenticationWS;
using Atlantis.Framework.AuthToken.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthToken.Impl
{
  public class AuthTokenRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthTokenResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase ))
        {
          throw new AtlantisException( oRequestData, "AuthToken.RequestHandler", "AuthToken WS URL in atlantis.config must use https.", string.Empty );
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          authenticationService.Url = authServiceUrl;
          authenticationService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

          AuthTokenRequestData request = (AuthTokenRequestData)oRequestData;
          HashSet<int> responseCodes = ValidateRequest(request);
          string authToken = String.Empty;
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            int resultCode = authenticationService.GetAuthToken(request.ShopperID, request.PrivateLabelId, out authToken, out errorOutput);
            responseCodes.Add(resultCode);
          }

          responseData = new AuthTokenResponseData(authToken, responseCodes, errorOutput);

        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthTokenResponseData( exAtlantis );
      }
      catch (Exception ex)
      {
        responseData = new AuthTokenResponseData( oRequestData, ex );
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthTokenRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region ShopperID
      if (string.IsNullOrEmpty( request.ShopperID ))
      {
        result.Add( AuthTokenStatusCodes.ShopperIdRequired );
      }
      #endregion

      return result;
    }
  }
}
