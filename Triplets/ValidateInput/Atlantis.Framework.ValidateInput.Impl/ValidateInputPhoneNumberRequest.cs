using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
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
        var errorResult = new ValidateInputResult(false, new List<int> { -1 });
        responseData = new ValidateInputPhoneNumberResponseData(errorResult, requestData, ex);
      }

      return responseData;
    }

    private bool ValidatePhoneNumber(IDictionary<string, string> inputs, out IList<int> errorCodes)
    {
      errorCodes = new List<int>();

      string inputPhoneNumber;
      if (!inputs.TryGetValue("phonenumber", out inputPhoneNumber) || string.IsNullOrEmpty(inputPhoneNumber))
      {
        errorCodes.Add(2);
      }
      else
      {
        var phoneUtil = PhoneNumberUtil.GetInstance();

        string regionCode;
        if (!inputs.TryGetValue("regioncode", out regionCode) || string.IsNullOrEmpty(regionCode))
        {
          string callingCode;
          if (inputs.TryGetValue("countrycallingcode", out callingCode) && !string.IsNullOrEmpty(callingCode))
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
              errorCodes.Add(8);
              break;
            case ErrorType.NOT_A_NUMBER:
              errorCodes.Add(7);
              break;
            case ErrorType.TOO_LONG:
              errorCodes.Add(6);
              break;
            case ErrorType.TOO_SHORT_AFTER_IDD:
            case ErrorType.TOO_SHORT_NSN:
              errorCodes.Add(5);
              break;
            default:
              errorCodes.Add(-1);
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
                errorCodes.Add(7);
              }
              break;
            case PhoneNumberUtil.ValidationResult.INVALID_COUNTRY_CODE:
              errorCodes.Add(8);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_LONG:
              errorCodes.Add(6);
              break;
            case PhoneNumberUtil.ValidationResult.TOO_SHORT:
              errorCodes.Add(5);
              break;
            default:
              errorCodes.Add(-1);
              break;
          }
        }
      }

      return (errorCodes.Count == 0);
    }
  }
}