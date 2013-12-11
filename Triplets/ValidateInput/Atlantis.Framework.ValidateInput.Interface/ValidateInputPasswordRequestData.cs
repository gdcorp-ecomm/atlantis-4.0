using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPasswordRequestData : RequestData
  {
    public IDictionary<ValidateInputKeys, string> Inputs { get; set; }

    public ValidateInputPasswordRequestData(IDictionary<ValidateInputKeys, string> inputs)
    {
      Inputs = inputs;
    }
  }
}