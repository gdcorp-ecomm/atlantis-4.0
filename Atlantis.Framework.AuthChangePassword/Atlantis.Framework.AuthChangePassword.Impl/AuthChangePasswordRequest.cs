using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthChangePassword.Impl.WSgdAuthentication;
using Atlantis.Framework.AuthChangePassword.Interface;
using Atlantis.Framework.AuthValidatePassword.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.AuthChangePassword.Impl
{
  public class AuthChangePasswordRequest : IRequest
  {
    private static readonly Regex _newHintInvalidCharactersRegex = new Regex("[^\x20-\x3b\x3f-\x7e]", RegexOptions.Compiled);

    private HashSet<int> ValidateRequest(AuthChangePasswordRequestData request, out List<ValidationFailure> regexErrors)
    {
      HashSet<int> result = new HashSet<int>();

      #region CurrentPassword

      if (string.IsNullOrEmpty(request.CurrentPassword))
      {
        result.Add(AuthChangePasswordStatusCodes.ValidatePasswordRequired);
      }
      else if (request.CurrentPassword.Length < 5)
      {
        result.Add(AuthChangePasswordStatusCodes.ValidateCurrentPasswordToShort);
      }

      #endregion

      #region NewPassword

      if (!ValidatePasswordFieldRegEx(request.NewPassword, request, out regexErrors))
      {
        result.Add(AuthChangePasswordStatusCodes.PasswordStrengthWeak);
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

        int login; //if login is all numbers then it MUST BE equal to shopper id
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
      AuthChangePasswordResponseData responseData;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) oConfig;
        string authServiceUrl = wsConfigElement.WSURL;
        if (!authServiceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(oRequestData, "AuthChangePassword.RequestHandler", "ChangePassword WS URL in atlantis.config must use https.", string.Empty);
        }

        AuthChangePasswordRequestData request = (AuthChangePasswordRequestData)oRequestData;

        X509Certificate2 cert = wsConfigElement.GetClientCertificate();
        cert.Verify();
        
        using (Authentication authenticationService = new Authentication { Url = authServiceUrl, Timeout = (int)request.RequestTimeout.TotalMilliseconds})
        {
          string statusMessage = string.Empty;
          long statusCode = TwoFactorWebserviceResponseCodes.Error;

          bool isPasswordChange = !(request.CurrentPassword == request.NewPassword); //when current = new we aren't changing password

          List<ValidationFailure> regexErrors; 

          HashSet<int> responseCodes = ValidateRequest(request, out regexErrors);

          if (responseCodes.Count > 0 || regexErrors.Count > 0)
          {
            statusMessage = "Request not valid.";
            responseData = new AuthChangePasswordResponseData(statusCode, statusMessage, responseCodes, regexErrors);
          }
          else
          {
            if (!ValidatePassword(request.NewPassword, request, ref responseCodes, ref statusMessage, ref statusCode))
            {
              if (statusMessage.Length == 0) { statusMessage = "Request not valid."; }
              responseData = new AuthChangePasswordResponseData(statusCode, statusMessage, responseCodes, regexErrors);
            }
            else
            {

              authenticationService.Url =  authServiceUrl;
              authenticationService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
              authenticationService.ClientCertificates.Add(cert);

              statusCode = authenticationService.ChangePassword(
                request.ShopperID, request.PrivateLabelId, request.CurrentPassword, request.NewPassword, request.AuthToken, request.PhoneNumber, request.HostName, request.IpAddress,
                request.NewHint, request.NewLogin, 1, out statusMessage);

              //we need to strip out the 30 day password reuse error if the user is not changing their pw
              if (!isPasswordChange)
              {
                responseCodes.Remove(AuthChangePasswordStatusCodes.PasswordStrengthAlreadyUsed);
              }

              responseData = new AuthChangePasswordResponseData(statusCode, statusMessage, responseCodes, regexErrors);
            }
          }
        }

      }
      catch (Exception ex)
      {
        responseData = new AuthChangePasswordResponseData(oRequestData, ex);
      }

      return responseData;
    }

    #endregion

    private bool ValidatePasswordFieldRegEx(string password, AuthChangePasswordRequestData oRequestData, out List<ValidationFailure> regexErrors)
    {

      regexErrors = new List<ValidationFailure>();
      bool isValid;

      try
      {
        ValidateFieldRequestData request = new ValidateFieldRequestData(oRequestData.ShopperID, oRequestData.SourceURL, oRequestData.OrderID, oRequestData.Pathway, oRequestData.PageCount, "password");

        //ValidateFieldResponseData response = (ValidateFieldResponseData)Engine.Engine.ProcessRequest(request, 507); //use this when debugging the framework item
        ValidateFieldResponseData response = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507); // use this for release version for your code
        
        isValid = response.ValidateStringField(password, out regexErrors);
      }
      catch
      {
        isValid = false;
      }

      return isValid;
    }

    private bool ValidatePassword(string password, AuthChangePasswordRequestData oRequestData, ref HashSet<int> responseCodes, ref string statusMessage, ref long statusCode)
    {
      bool isValid;

      try
      {
        AuthValidatePasswordRequestData request = new AuthValidatePasswordRequestData(oRequestData.ShopperID, oRequestData.SourceURL, oRequestData.OrderID, oRequestData.Pathway, oRequestData.PageCount, password);

        AuthValidatePasswordResponseData response = (AuthValidatePasswordResponseData)Engine.Engine.ProcessRequest(request, 517);

        isValid = response.IsPasswordValid;
        statusCode = response.StatusCode;
        responseCodes = response.ValidationCodes;
        statusMessage = response.StatusMessage;
      }
      catch
      {
        isValid = false;
      }

      return isValid;
    }
  }
}
