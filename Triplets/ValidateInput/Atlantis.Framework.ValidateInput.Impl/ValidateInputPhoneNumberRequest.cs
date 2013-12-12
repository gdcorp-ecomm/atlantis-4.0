using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;
using Atlantis.Framework.ValidateInput.Interface;
using PhoneNumbers;

namespace Atlantis.Framework.ValidateInput.Impl
{
  public class ValidateInputPhoneNumberRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ValidateInputPhoneNumberResponseData responseData;

      try
      {
        var validateInputPhoneNumberRequestData = (ValidateInputPhoneNumberRequestData)requestData;
        var result = ValidateInputResult.CreateFailureResult();

        if (validateInputPhoneNumberRequestData.Inputs != null)
        {
          IList<int> errorCodes;
          var isSuccess = ValidatePhoneNumber(validateInputPhoneNumberRequestData.Inputs, out errorCodes);
          result = new ValidateInputResult(isSuccess, errorCodes);
        }

        responseData = new ValidateInputPhoneNumberResponseData(result);
      }
      catch (Exception ex)
      {
        var errorResult = new ValidateInputResult(false, new List<int> { PasswordErrorCodes.UnknownError });
        responseData = new ValidateInputPhoneNumberResponseData(errorResult, requestData, ex);
      }

      return responseData;
    }

    private bool ValidatePhoneNumber(IDictionary<ValidateInputKeys, string> inputs, out IList<int> errorCodes)
    {
      errorCodes = new List<int>();

      string inputPhoneNumber;
      if (!inputs.TryGetValue(ValidateInputKeys.PhoneNumberInput, out inputPhoneNumber) || string.IsNullOrEmpty(inputPhoneNumber))
      {
        errorCodes.Add(PhoneNumberErrorCodes.PhoneNumberEmpty);
      }
      else
      {
        var phoneUtil = PhoneNumberUtil.GetInstance();

        string regionCode;
        if (!inputs.TryGetValue(ValidateInputKeys.PhoneNumberRegionCode, out regionCode) || string.IsNullOrEmpty(regionCode))
        {
          string callingCode;
          if (inputs.TryGetValue(ValidateInputKeys.PhoneNumberCountryCallingCode, out callingCode) && !string.IsNullOrEmpty(callingCode))
          {
            int code;
            if (int.TryParse(callingCode, out code))
            {
              regionCode = phoneUtil.GetRegionCodeForCountryCode(code);
            }
          }
        }

        if (string.IsNullOrEmpty(regionCode) && !inputPhoneNumber.StartsWith("+"))
        {
          inputPhoneNumber = string.Format("+{0}", inputPhoneNumber);
        }

        PhoneNumber number = null;
        try
        {
          number = phoneUtil.Parse(inputPhoneNumber, regionCode);
        }
        catch (NumberParseException ex)
        {
          switch (ex.ErrorType)
          {
            case ErrorType.INVALID_COUNTRY_CODE:
              errorCodes.Add(PhoneNumberErrorCodes.InvalidCountryCode);
              break;
            case ErrorType.NOT_A_NUMBER:
              errorCodes.Add(PhoneNumberErrorCodes.InvalidPhoneNumber);
              break;
            case ErrorType.TOO_LONG:
              errorCodes.Add(PhoneNumberErrorCodes.TooLong);
              break;
            case ErrorType.TOO_SHORT_AFTER_IDD:
            case ErrorType.TOO_SHORT_NSN:
              errorCodes.Add(PhoneNumberErrorCodes.TooShort);
              break;
            default:
              errorCodes.Add(PhoneNumberErrorCodes.UnknownError);
              break;
          }
        }

        if (number != null)
        {
          switch (phoneUtil.IsPossibleNumberWithReason(number))
          {
            case PhoneNumberUtil.ValidationResult.IS_POSSIBLE:
              if (!phoneUtil.IsValidNumber(number))
              {
                errorCodes.Add(PhoneNumberErrorCodes.InvalidPhoneNumber);
              }
              break;
            case PhoneNumberUtil.ValidationResult.INVALID_COUNTRY_CODE:
              errorCodes.Add(PhoneNumberErrorCodes.InvalidCountryCode);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_LONG:
              errorCodes.Add(PhoneNumberErrorCodes.TooLong);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_SHORT:
              errorCodes.Add(PhoneNumberErrorCodes.TooShort);
              break;
            default:
              errorCodes.Add(PhoneNumberErrorCodes.UnknownError);
              break;
          }
        }
      }

      return (errorCodes.Count == 0);
    }
  }
}