using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputProvider
  {
    IValidateInputResult ValidateInput(ValidateInputTypes inputType, IDictionary<ValidateInputKeys, string> inputs);
  }
}
