using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPhoneNumberRequestData : RequestData
  {
    public IDictionary<string, string> Inputs { get; set; }

    public ValidateInputPhoneNumberRequestData(IDictionary<string, string> inputs)
    {
      Inputs = inputs;
    }
  }
}