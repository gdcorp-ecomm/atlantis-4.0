﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Factories;
using Atlantis.Framework.Providers.ValidateInput.Interface;

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

      if (inputs == null) return result;

      var handler = ValidateInputFactory.GetHandler(inputType);

      if (handler == null) return result;

      try
      {
        result = handler.Validate(inputs);
      }
      catch (Exception ex)
      {
        var data = "inputType: " + inputType + ", inputs: " + string.Join(",", inputs);
        var exception = new AtlantisException("ValidateInputProvider.ValidateInput", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}