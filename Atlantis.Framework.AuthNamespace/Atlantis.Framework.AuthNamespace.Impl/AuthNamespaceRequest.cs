using System;
using System.Collections.Generic;

using Atlantis.Framework.AuthNamespace.Impl.AuthenticationWS;
using Atlantis.Framework.AuthNamespace.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthNamespace.Impl
{
  public class AuthNamespaceRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthNamespaceResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase ))
        {
          throw new AtlantisException( oRequestData, "AuthNamespace.RequestHandler", "AuthNamespace WS URL in atlantis.config must use https.", string.Empty );
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          authenticationService.Url = authServiceUrl;

          AuthNamespaceRequestData request = (AuthNamespaceRequestData)oRequestData;
          authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          HashSet<int> responseCodes = ValidateRequest(request);
          string shopperId = String.Empty;
          string email = String.Empty;
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            shopperId = authenticationService.LookupNamespace(request.Namespace, request.Key, request.PrivateLabelId, out email, out errorOutput);
            int resultCode;
            if (!String.IsNullOrEmpty(shopperId))
            {
              resultCode = AuthNamespaceStatusCodes.Success;
            }
            else
            {
              resultCode = String.IsNullOrEmpty(errorOutput) ? AuthNamespaceStatusCodes.Failure :
                AuthNamespaceStatusCodes.Error;
            }
            responseCodes.Add(resultCode);
          }

          responseData = new AuthNamespaceResponseData(shopperId, email, responseCodes, errorOutput);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthNamespaceResponseData( exAtlantis );
      }
      catch (Exception ex)
      {
        responseData = new AuthNamespaceResponseData( oRequestData, ex );
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthNamespaceRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region Namespace
      if (string.IsNullOrEmpty( request.Namespace ))
      {
        result.Add( AuthNamespaceStatusCodes.ValidateNamespaceRequired );
      }
      #endregion

      #region Key
      if (string.IsNullOrEmpty( request.Key ))
      {
        result.Add( AuthNamespaceStatusCodes.ValidateKeyRequired );
      }
      #endregion

      return result;
    }
  }
}
