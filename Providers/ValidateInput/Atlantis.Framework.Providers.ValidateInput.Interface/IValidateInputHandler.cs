using System.Collections.Generic;
using Atlantis.Framework.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputHandler
  {
    IValidateInputResult Validate(IDictionary<ValidateInputKeys, string> inputs);
  }
}