using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput.Handlers
{
  public class ValidateInputPhoneNumberHandler : IValidateInputHandler
  {
    public IValidateInputResult Validate(IDictionary<ValidateInputKeys, string> inputs)
    {
      IValidateInputResult result = ValidateInputResult.CreateFailureResult();

      if (inputs == null) return result;

      try
      {
        var request = new ValidateInputPhoneNumberRequestData(inputs);
        var response = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, ValidateInputEngineRequests.ValidateInputPhoneNumberRequest);
        result = ValidateInputResult.GetValidateInputResult(response.Result);
      }
      catch (Exception ex)
      {
        var data = "inputs: " + string.Join(",", inputs);
        var exception = new AtlantisException("ValidateInputPhoneNumberHandler.Validate", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}