using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.AuthChangePassword.Impl.AuthenticationWS;
using Atlantis.Framework.AuthChangePassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthChangePassword.Impl
{
  public class AuthChangePasswordRequest : IRequest
  {
    private static readonly Regex _newPasswordInvalidCharactersRegex = new Regex("[^\x20-\x7E]", RegexOptions.Compiled);
    private static readonly Regex _newLoginInvalidCharactersRegex = new Regex(@"[^\-A-Za-z0-9]", RegexOptions.Compiled);
    private static readonly Regex _newHintInvalidCharactersRegex = new Regex("[^\x20-\x3b\x3f-\x7e]", RegexOptions.Compiled);

    private HashSet<int> ValidateRequest(AuthChangePasswordRequestData request, WScgdAuthenticateService service)
    {
      HashSet<int> result = new HashSet<int>();

      #region CurrentPassword

      if (string.IsNullOrEmpty(request.CurrentPassword))
      {
        result.Add(AuthChangePasswordStatusCodes.CurrentPasswordRequired);
      }
      else if (request.CurrentPassword.Length < 5)
      {
        result.Add(AuthChangePasswordStatusCodes.CurrentPasswordToShort);
      }

      #endregion

      #region NewPassword

      if (string.IsNullOrEmpty(request.NewPassword))
      {
        result.Add(AuthChangePasswordStatusCodes.PasswordRequired);
      }
      else
      {
        if (request.NewPassword.Length > 25)
        {
          result.Add(AuthChangePasswordStatusCodes.PasswordToLong);
        }
        else if (request.NewPassword.Length < 5)
        {
          result.Add(AuthChangePasswordStatusCodes.PasswordToShort);
        }

        if (_newPasswordInvalidCharactersRegex.Match(request.NewPassword).Success)
        {
          result.Add(AuthChangePasswordStatusCodes.PasswordInvalidCharacters);
        }
        else if (request.UseStrongPassword)
        {
          // Validate password strength
          int strengthResult = service.IsStrongPassword(request.ShopperID, request.NewPassword);
          if (strengthResult != AuthChangePasswordStatusCodes.Success)
          {
            result.Add(strengthResult);
          }
        }
      }

      #endregion

      #region Login

      if (string.IsNullOrEmpty(request.NewLogin))
      {
        result.Add(AuthChangePasswordStatusCodes.LoginRequired);
      }
      else
      {
        if (request.NewLogin.Length > 30)
        {
          result.Add(AuthChangePasswordStatusCodes.LoginMaxLength);
        }

        if (_newLoginInvalidCharactersRegex.Match(request.NewLogin).Success)
        {
          result.Add(AuthChangePasswordStatusCodes.LoginInvalidCharacters);
        }
      }

      #endregion

      #region Hint

      if (string.IsNullOrEmpty(request.NewHint))
      {
        result.Add(AuthChangePasswordStatusCodes.HintRequired);
      }
      else
      {
        if (request.NewHint.Length > 255)
        {
          result.Add(AuthChangePasswordStatusCodes.HintMaxLength);
        }

        if (_newHintInvalidCharactersRegex.Match(request.NewHint).Success)
        {
          result.Add(AuthChangePasswordStatusCodes.HintInvalidCharacters);
        }
      }

      #endregion

      #region Cross-Field Rules

      if (request.NewLogin == request.NewPassword)
      {
        result.Add(AuthChangePasswordStatusCodes.LoginPasswordMatch);
      }

      if (request.NewLogin == request.NewHint)
      {
        result.Add(AuthChangePasswordStatusCodes.LoginHintMatch);
      }

      if (request.NewHint == request.NewPassword)
      {
        result.Add(AuthChangePasswordStatusCodes.PasswordHintMatch);
      }

      #endregion

      return result;
    }

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuthChangePasswordResponseData responseData = null;

      try
      {
        string authServiceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(oRequestData, "AuthChangePassword.RequestHandler", "ChangePassword WS URL in atlantis.config must use https.", string.Empty);
        }

        AuthChangePasswordRequestData request = (AuthChangePasswordRequestData)oRequestData;
        WScgdAuthenticateService authenticationService = new WScgdAuthenticateService()
                                                           {
                                                             Url = authServiceUrl,
                                                             Timeout = (int) request.RequestTimeout.TotalMilliseconds
                                                           };

        string errorOutput = null;

        HashSet<int> responseCodes = ValidateRequest(request, authenticationService);
        if (responseCodes.Count > 0)
        {
          errorOutput = "Request not valid.";
        }
        else
        {
          int useStrongPasswordValue = 0;
          if (request.UseStrongPassword)
          {
            useStrongPasswordValue = 1;
          }

          int resultCode = authenticationService.ChangePassword(
            request.ShopperID, request.PrivateLabelId, request.CurrentPassword, request.NewPassword,
            request.NewHint, request.NewLogin, useStrongPasswordValue, out errorOutput);

          responseCodes.Add(resultCode);
        }

        responseData = new AuthChangePasswordResponseData(responseCodes, errorOutput);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthChangePasswordResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new AuthChangePasswordResponseData(oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
