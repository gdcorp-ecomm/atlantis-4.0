using System;
using System.Collections.Generic;

using Atlantis.Framework.AuthHint.Impl.AuthenticationWS;
using Atlantis.Framework.AuthHint.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthHint.Impl
{
  public class AuthHintRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthHintResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase ))
        {
          throw new AtlantisException( oRequestData, "AuthHint.RequestHandler", "AuthHint WS URL in atlantis.config must use https.", string.Empty );
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          authenticationService.Url = authServiceUrl;

          AuthHintRequestData request = (AuthHintRequestData)oRequestData;
          authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          HashSet<int> responseCodes = ValidateRequest(request);
          string hint = String.Empty;
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            int resultCode = authenticationService.GetHint(request.LoginName, request.PrivateLabelId, request.Street, out hint, out errorOutput);
            responseCodes.Add(resultCode);
          }

          responseData = new AuthHintResponseData(hint, responseCodes, errorOutput);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthHintResponseData( exAtlantis );
      }
      catch (Exception ex)
      {
        responseData = new AuthHintResponseData( oRequestData, ex );
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthHintRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region LoginName
      if (string.IsNullOrEmpty( request.LoginName ))
      {
        result.Add( AuthHintStatusCodes.LoginNameRequired );
      }
      #endregion

      #region Street
      if (string.IsNullOrEmpty( request.Street ))
      {
        result.Add( AuthHintStatusCodes.StreetRequired );
      }
      #endregion

      return result;
    }
  }
}
