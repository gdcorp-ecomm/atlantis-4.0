using System.Collections.Generic;
using Atlantis.Framework.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputProvider
  {
    IValidateInputResult ValidateInput(ValidateInputTypes inputType, IDictionary<ValidateInputKeys, string> inputs);
  }
}
