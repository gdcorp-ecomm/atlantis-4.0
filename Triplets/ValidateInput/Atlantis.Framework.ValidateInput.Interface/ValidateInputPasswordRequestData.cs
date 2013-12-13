using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPasswordRequestData : RequestData
  {
    public IDictionary<string, string> Inputs { get; set; }

    public ValidateInputPasswordRequestData(IDictionary<string, string> inputs)
    {
      Inputs = inputs;
    }
  }
}