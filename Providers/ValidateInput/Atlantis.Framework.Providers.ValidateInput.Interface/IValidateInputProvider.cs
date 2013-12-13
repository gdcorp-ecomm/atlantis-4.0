using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputProvider
  {
    IValidateInputResult ValidateInput(ValidateInputTypes inputType, IDictionary<string, string> inputs);
  }
}
