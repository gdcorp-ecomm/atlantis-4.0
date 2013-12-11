using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputHandler
  {
    IValidateInputResult Validate(IDictionary<ValidateInputKeys, string> inputs);
  }
}