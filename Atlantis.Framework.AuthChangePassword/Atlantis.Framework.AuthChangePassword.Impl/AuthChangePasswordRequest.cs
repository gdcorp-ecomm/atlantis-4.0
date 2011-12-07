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
    private static readonly Regex _newHintInvalidCharactersRegex = new Regex("[^\x20-\x3b\x3f-\x7e]", RegexOptions.Compiled);
    private static readonly Regex _meetsStrongPwReqs = new Regex("(?=.*[A-Z])(?=.{8,})(?=.*\\d).*$");

    private HashSet<int> ValidateRequest(AuthChangePasswordRequestData request, WScgdAuthenticateService service, bool isPasswordChange)
    {
      HashSet<int> result = new HashSet<int>();

      #region CurrentPassword

      if (string.IsNullOrEmpty(request.CurrentPassword))
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateCurrentPasswordRequired);
      }
      else if (request.CurrentPassword.Length < 5)
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateCurrentPasswordToShort);
      }
      else if (request.UseStrongPassword && !_meetsStrongPwReqs.IsMatch(request.CurrentPassword) && !isPasswordChange)
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateCurrentPasswordMeetsRequirements);
      }

      #endregion

      #region NewPassword

      if (string.IsNullOrEmpty(request.NewPassword))
      {
        result.Add(AuthChangePasswordStatusCodes.ValidatePasswordRequired);
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
          result.Add(AuthChangePasswordStatusCodes.ValidatePasswordInvalidCharacters);
        }
        else if (request.UseStrongPassword)
        {
          // Validate password strength
          int strengthResult = service.IsStrongPassword(request.ShopperID, request.NewPassword);

          //we need to strip out the 30 day password reuse error if the user is not changing their pw
          bool ignoreReusePwError = (!isPasswordChange && strengthResult == AuthChangePasswordStatusCodes.PasswordStrengthAlreadyUsed);

          if (strengthResult != AuthChangePasswordStatusCodes.Success && !ignoreReusePwError)
          {
            result.Add(strengthResult);
          }
        }
      }

      #endregion

      #region Login

      if (string.IsNullOrEmpty(request.NewLogin))
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateLoginRequired);
      }
      else
      {
        if (request.NewLogin.Length > 30)
        {
          result.Add(AuthChangePasswordStatusCodes.ValidateLoginMaxLength);
        }

        int login = 0; //if login is all numbers then it MUST BE equal to shopper id
        if (int.TryParse(request.NewLogin, out login))
        {
          if (request.ShopperID != request.NewLogin)
          {
            result.Add(AuthChangePasswordStatusCodes.ValidateLoginMustBeEqualToId);
          }
        }
      }

      #endregion

      #region Hint

      if (string.IsNullOrEmpty(request.NewHint))
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateHintRequired);
      }
      else
      {
        if (request.NewHint.Length > 255)
        {
          result.Add(AuthChangePasswordStatusCodes.ValidateHintMaxLength);
        }

        if (_newHintInvalidCharactersRegex.Match(request.NewHint).Success)
        {
          result.Add(AuthChangePasswordStatusCodes.ValidateHintInvalidCharacters);
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
        using (WScgdAuthenticateService authenticationService = new WScgdAuthenticateService()
                                                           {
                                                             Url = authServiceUrl,
                                                             Timeout = (int)request.RequestTimeout.TotalMilliseconds
                                                           })
        {
          string errorOutput = null;
          bool isPasswordChange = !(request.CurrentPassword == request.NewPassword); //when current = new we aren't changing password

          HashSet<int> responseCodes = ValidateRequest(request, authenticationService, isPasswordChange);          

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

            //we need to strip out the 30 day password reuse error if the user is not changing their pw
            if (!isPasswordChange)
            {
              responseCodes.Remove(AuthChangePasswordStatusCodes.PasswordStrengthAlreadyUsed);
              if (responseCodes.Count == 0)
              {
                responseCodes.Add(AuthChangePasswordStatusCodes.Success);
              }
            }
          }       
          
          responseData = new AuthChangePasswordResponseData(responseCodes, errorOutput);
        }

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
