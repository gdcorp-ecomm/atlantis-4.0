using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
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
        var errorResult = new ValidateInputResult(false, new List<int> { -1 });
        responseData = new ValidateInputPasswordResponseData(errorResult, requestData, ex);
      }

      return responseData;
    }

    private bool ValidatePassword(IDictionary<string, string> inputs, out IList<int> errorCodes)
    {
      errorCodes = new List<int>();

      string inputPassword;
      if (!inputs.TryGetValue("password", out inputPassword) || string.IsNullOrEmpty(inputPassword))
      {
        errorCodes.Add(5);
      }
      else
      {
        string fieldValidationXml;

        if (!Data.FieldValidationData.TryGetFieldValidationXml("password", out fieldValidationXml) || string.IsNullOrEmpty(fieldValidationXml))
        {
          errorCodes.Add(4);
        }
        else
        {
          var fieldValidationDoc = XDocument.Parse(fieldValidationXml);

          if (fieldValidationDoc.Root == null)
          {
            errorCodes.Add(4);
          }
          else
          {
            string inputPasswordMatch;
            if (inputs.TryGetValue("passwordconfirmation", out inputPasswordMatch) && !inputPasswordMatch.Equals(inputPassword))
            {
              errorCodes.Add(6);
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