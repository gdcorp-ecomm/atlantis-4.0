using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Atlantis.Framework.AuthResetPassword.Impl.AuthenticationWS;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Impl
{
  public class AuthResetPasswordRequest : IRequest
  {
    public IResponseData RequestHandler( RequestData oRequestData, ConfigElement oConfig )
    {
      AuthResetPasswordResponseData responseData;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase ))
        {
          throw new AtlantisException( oRequestData, "AuthResetPassword.RequestHandler", "AuthResetPassword WS URL in atlantis.config must use https.", string.Empty );
        }

        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService())
        {
          authenticationService.Url = authServiceUrl;
          authenticationService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

          AuthResetPasswordRequestData request = (AuthResetPasswordRequestData)oRequestData;
          HashSet<int> responseCodes = ValidateRequest(request);
          string errorOutput;

          if (responseCodes.Count > 0)
          {
            errorOutput = "Request not valid.";
          }
          else
          {
            int resultCode = authenticationService.ResetPassword(request.ShopperID, request.PrivateLabelId, request.IpAddress, request.NewPassword, request.NewHint, request.AuthToken, out errorOutput);
            responseCodes.Add(resultCode);
          }

          responseData = new AuthResetPasswordResponseData(responseCodes, errorOutput);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthResetPasswordResponseData( exAtlantis );
      }
      catch (Exception ex)
      {
        responseData = new AuthResetPasswordResponseData( oRequestData, ex );
      }

      return responseData;
    }

    private static HashSet<int> ValidateRequest( AuthResetPasswordRequestData request )
    {
      HashSet<int> result = new HashSet<int>();

      #region ShopperID
      if (string.IsNullOrEmpty( request.ShopperID ))
      {
        result.Add( AuthResetPasswordStatusCodes.ShopperIdRequired );
      }
      #endregion

      #region IpAddress
      if (string.IsNullOrEmpty( request.IpAddress ))
      {
        result.Add( AuthResetPasswordStatusCodes.IpAddressRequired );
      }
      #endregion

      #region NewPassword
      if (string.IsNullOrEmpty( request.NewPassword ))
      {
        result.Add( AuthResetPasswordStatusCodes.PasswordRequired );
      }
      else
      {
        if (request.NewPassword.Length > 25)
        {
          result.Add( AuthResetPasswordStatusCodes.PasswordTooLong );
        }
        else if (request.NewPassword.Length < 5)
        {
          result.Add( AuthResetPasswordStatusCodes.PasswordTooShort );
        }

        if (Regex.Match( request.NewPassword, "[^\x20-\x7E]" ).Success)
        {
          result.Add( AuthResetPasswordStatusCodes.PasswordInvalidCharacters );
        }
      }
      #endregion

      #region NewHint
      if (string.IsNullOrEmpty( request.NewHint ))
      {
        result.Add( AuthResetPasswordStatusCodes.HintRequired );
      }
      else
      {
        if (request.NewHint.Length > 255)
        {
          result.Add( AuthResetPasswordStatusCodes.HintMaxLength );
        }

        if (Regex.Match( request.NewHint, "[^\x20-\x3b\x3f-\x7e]" ).Success)
        {
          result.Add( AuthResetPasswordStatusCodes.HintInvalidCharacters );
        }
      }
      #endregion

      #region AuthToken
      if (String.IsNullOrEmpty( request.AuthToken ))
      {
        result.Add( AuthResetPasswordStatusCodes.AuthTokenRequired );
      }
      #endregion

      #region Cross-Field Rules
      if (request.NewHint == request.NewPassword)
      {
        result.Add( AuthResetPasswordStatusCodes.PasswordHintMatch );
      }
      #endregion

      return result;
    }
  }
}
