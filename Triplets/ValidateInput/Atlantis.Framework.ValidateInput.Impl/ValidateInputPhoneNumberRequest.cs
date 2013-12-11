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
    private bool _isSuccess = false;
    private List<int> _errorCodes = new List<int>();

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var validateInputPhoneNumberRequestData = (ValidateInputPhoneNumberRequestData)requestData;
      ValidateInputPhoneNumberResponseData responseData;
      var result = ValidateInputResult.CreateFailureResult();

      try
      {
        if (validateInputPhoneNumberRequestData.Inputs != null)
        {
          ValidatePhoneNumber(validateInputPhoneNumberRequestData.Inputs);
          result = new ValidateInputResult(_isSuccess, _errorCodes);
        }

        responseData = new ValidateInputPhoneNumberResponseData(result);
      }
      catch (Exception ex)
      {
        _isSuccess = false;
        _errorCodes.Add(PhoneNumberErrorCodes.UnknownError);
        result = new ValidateInputResult(_isSuccess, _errorCodes);
        responseData = new ValidateInputPhoneNumberResponseData(result, requestData, ex);
      }

      return responseData;
    }

    private void ValidatePhoneNumber(IDictionary<ValidateInputKeys, string> inputs)
    {
      string inputPhoneNumber;
      if (!inputs.TryGetValue(ValidateInputKeys.PhoneNumberInput, out inputPhoneNumber) || string.IsNullOrEmpty(inputPhoneNumber))
      {
        _errorCodes.Add(PhoneNumberErrorCodes.EmptyPhoneNumber);
      }
      else
      {
        var phoneUtil = PhoneNumberUtil.GetInstance();

        string regionCode;
        if (!inputs.TryGetValue(ValidateInputKeys.PhoneNumberRegionCode, out regionCode) || string.IsNullOrEmpty(regionCode))
        {
          string callingCode;
          if (inputs.TryGetValue(ValidateInputKeys.PhoneNumberCountryCallingCode, out callingCode) && !string.IsNullOrEmpty(regionCode))
          {
            int code;
            if (int.TryParse(callingCode, out code))
            {
              regionCode = phoneUtil.GetRegionCodeForCountryCode(code);
            }
          }
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
              _errorCodes.Add(PhoneNumberErrorCodes.InvalidCountryCode);
              break;
            case ErrorType.NOT_A_NUMBER:
              _errorCodes.Add(PhoneNumberErrorCodes.InvalidPhoneNumber);
              break;
            case ErrorType.TOO_LONG:
              _errorCodes.Add(PhoneNumberErrorCodes.TooLong);
              break;
            case ErrorType.TOO_SHORT_AFTER_IDD:
            case ErrorType.TOO_SHORT_NSN:
              _errorCodes.Add(PhoneNumberErrorCodes.TooShort);
              break;
            default:
              _errorCodes.Add(PhoneNumberErrorCodes.UnknownError);
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
                _errorCodes.Add(PhoneNumberErrorCodes.InvalidPhoneNumber);
              }
              break;
            case PhoneNumberUtil.ValidationResult.INVALID_COUNTRY_CODE:
              _errorCodes.Add(PhoneNumberErrorCodes.InvalidCountryCode);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_LONG:
              _errorCodes.Add(PhoneNumberErrorCodes.TooLong);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_SHORT:
              _errorCodes.Add(PhoneNumberErrorCodes.TooShort);
              break;
            default:
              _errorCodes.Add(PhoneNumberErrorCodes.UnknownError);
              break;
          }
        }
      }

      _isSuccess = (_errorCodes.Count == 0);
    }
  }
}