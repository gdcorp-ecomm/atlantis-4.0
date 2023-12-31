﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;
using Atlantis.Framework.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput.Handlers
{
  public class ValidateInputPhoneNumberHandler : IValidateInputHandler
  {
    public IValidateInputResult Validate(IDictionary<string, string> inputs)
    {
      IValidateInputResult result = ValidateInputResult.CreateFailureResult();

      if (inputs == null || inputs.Count == 0)
      {
        result.ErrorCodes.Add(ErrorCodesBase.NoInputs);
        return result;
      }

      try
      {
        var request = new ValidateInputPhoneNumberRequestData(inputs);
        var response = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, ValidateInputEngineRequests.ValidateInputPhoneNumberRequest);
        result = ValidateInputResult.GetValidateInputResult(response.Result);
      }
      catch (Exception ex)
      {
        result.ErrorCodes.Add(ErrorCodesBase.UnknownError);
        var exception = new AtlantisException("ValidateInputPhoneNumberHandler.Validate", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}