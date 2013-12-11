using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;
using Atlantis.Framework.ValidateInput.Impl.Data;
using Atlantis.Framework.ValidateInput.Interface;

namespace Atlantis.Framework.ValidateInput.Impl
{
  public class ValidateInputPasswordRequest : IRequest
  {
    private bool _isSuccess = false;
    private List<int> _errorCodes = new List<int>();

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var validateInputPasswordRequestData = (ValidateInputPasswordRequestData)requestData;
      ValidateInputPasswordResponseData responseData;
      var result = ValidateInputResult.CreateFailureResult();

      try
      {
        if (validateInputPasswordRequestData.Inputs != null)
        {
          ValidatePassword(validateInputPasswordRequestData.Inputs);
          result = new ValidateInputResult(_isSuccess, _errorCodes);
        }

        responseData = new ValidateInputPasswordResponseData(result);
      }
      catch (Exception ex)
      {
        _isSuccess = false;
        _errorCodes.Add(PasswordErrorCodes.UnknownError);
        result = new ValidateInputResult(_isSuccess, _errorCodes);
        responseData = new ValidateInputPasswordResponseData(result, requestData, ex);
      }

      return responseData;
    }

    private void ValidatePassword(IDictionary<ValidateInputKeys, string> inputs)
    {
      string inputPassword;
      if (!inputs.TryGetValue(ValidateInputKeys.PasswordInput, out inputPassword) || string.IsNullOrEmpty(inputPassword))
      {
        _errorCodes.Add(PasswordErrorCodes.EmptyPassword);
      }
      else
      {
        string fieldValidationXml;

        if (!FieldValidationData.TryGetFieldValidationXml("password", out fieldValidationXml) || string.IsNullOrEmpty(fieldValidationXml))
        {
          _errorCodes.Add(PasswordErrorCodes.ValidationRulesLoadError);
        }
        else
        {
          var fieldValidationDoc = XDocument.Parse(fieldValidationXml);

          if (fieldValidationDoc.Root == null)
          {
            _errorCodes.Add(PasswordErrorCodes.ValidationRulesLoadError);
          }
          else
          {
            string inputPasswordMatch;
            if (inputs.TryGetValue(ValidateInputKeys.PasswordInputMatch, out inputPasswordMatch) && !inputPasswordMatch.Equals(inputPassword))
            {
              _errorCodes.Add(PasswordErrorCodes.PasswordsNotEqual);
            }

            var lengthRuleElement = fieldValidationDoc.Root.Descendants("length").FirstOrDefault();
            if (lengthRuleElement != null)
            {
              var lengthRule = new ValidationRuleLength(lengthRuleElement);

              if (!lengthRule.IsValid(inputPassword))
              {
                _errorCodes.Add(lengthRule.FailureCode);
              }
            }

            var expressionRuleElement = fieldValidationDoc.Root.Descendants("regex").FirstOrDefault();
            if (expressionRuleElement != null)
            {
              var expressionRule = new ValidationRuleRegEx(expressionRuleElement);

              if (!expressionRule.IsValid(inputPassword))
              {
                _errorCodes.Add(expressionRule.FailureCode);
              }
            }
          }
        }
      }

      _isSuccess = (_errorCodes.Count == 0);
    }
  }
}