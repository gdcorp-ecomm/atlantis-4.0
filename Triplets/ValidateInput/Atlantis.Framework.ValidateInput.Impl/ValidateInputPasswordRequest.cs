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
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ValidateInputPasswordResponseData responseData;

      try
      {
        var validateInputPasswordRequestData = (ValidateInputPasswordRequestData)requestData;
        var result = ValidateInputResult.CreateFailureResult();

        if (validateInputPasswordRequestData.Inputs != null)
        {
          IList<int> errorCodes;
          var isSuccess = ValidatePassword(validateInputPasswordRequestData.Inputs, out errorCodes);
          result = new ValidateInputResult(isSuccess, errorCodes);
        }

        responseData = new ValidateInputPasswordResponseData(result);
      }
      catch (Exception ex)
      {
        var errorResult = new ValidateInputResult(false, new List<int> { PasswordErrorCodes.UnknownError });
        responseData = new ValidateInputPasswordResponseData(errorResult, requestData, ex);
      }

      return responseData;
    }

    private bool ValidatePassword(IDictionary<ValidateInputKeys, string> inputs, out IList<int> errorCodes)
    {
      errorCodes = new List<int>();

      string inputPassword;
      if (!inputs.TryGetValue(ValidateInputKeys.PasswordInput, out inputPassword) || string.IsNullOrEmpty(inputPassword))
      {
        errorCodes.Add(PasswordErrorCodes.PasswordEmpty);
      }
      else
      {
        string fieldValidationXml;

        if (!FieldValidationData.TryGetFieldValidationXml("password", out fieldValidationXml) || string.IsNullOrEmpty(fieldValidationXml))
        {
          errorCodes.Add(PasswordErrorCodes.ValidationRulesLoadError);
        }
        else
        {
          var fieldValidationDoc = XDocument.Parse(fieldValidationXml);

          if (fieldValidationDoc.Root == null)
          {
            errorCodes.Add(PasswordErrorCodes.ValidationRulesLoadError);
          }
          else
          {
            string inputPasswordMatch;
            if (inputs.TryGetValue(ValidateInputKeys.PasswordInputMatch, out inputPasswordMatch) && !inputPasswordMatch.Equals(inputPassword))
            {
              errorCodes.Add(PasswordErrorCodes.PasswordsNotEqual);
            }

            var lengthRuleElement = fieldValidationDoc.Root.Descendants("length").FirstOrDefault();
            if (lengthRuleElement != null)
            {
              var lengthRule = new ValidationRuleLength(lengthRuleElement);

              if (!lengthRule.IsValid(inputPassword))
              {
                errorCodes.Add(lengthRule.FailureCode);
              }
            }

            var expressionRuleElement = fieldValidationDoc.Root.Descendants("regex").FirstOrDefault();
            if (expressionRuleElement != null)
            {
              var expressionRule = new ValidationRuleRegEx(expressionRuleElement);

              if (!expressionRule.IsValid(inputPassword))
              {
                errorCodes.Add(expressionRule.FailureCode);
              }
            }
          }
        }
      }

      return (errorCodes.Count == 0);
    }
  }
}