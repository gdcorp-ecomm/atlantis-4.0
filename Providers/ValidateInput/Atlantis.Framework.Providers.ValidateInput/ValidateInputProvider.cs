using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Factories;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;

namespace Atlantis.Framework.Providers.ValidateInput
{
  public class ValidateInputProvider : ProviderBase, IValidateInputProvider
  {
    public ValidateInputProvider(IProviderContainer container)
      : base(container)
    {
    }

    public IValidateInputResult ValidateInput(ValidateInputTypes inputType, IDictionary<ValidateInputKeys, string> inputs)
    {
      IValidateInputResult result = ValidateInputResult.CreateFailureResult();

      if (inputs == null || inputs.Count == 0)
      {
        result.ErrorCodes.Add(ErrorCodesBase.NoInputs);
        return result;
      }

      var handler = ValidateInputFactory.GetHandler(inputType);

      if (handler == null)
      {
        result.ErrorCodes.Add(ErrorCodesBase.InvalidInputType);
        return result;
      }

      try
      {
        result = handler.Validate(inputs);
      }
      catch (Exception ex)
      {
        result.ErrorCodes.Add(ErrorCodesBase.UnknownError);
        var data = "inputType: " + inputType;
        var exception = new AtlantisException("ValidateInputProvider.ValidateInput", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}