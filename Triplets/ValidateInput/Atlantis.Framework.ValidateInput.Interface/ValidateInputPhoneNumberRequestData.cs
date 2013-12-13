using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPhoneNumberRequestData : RequestData
  {
    public IDictionary<ValidateInputKeys, string> Inputs { get; set; }

    public ValidateInputPhoneNumberRequestData(IDictionary<ValidateInputKeys, string> inputs)
    {
      Inputs = inputs;
    }
  }
}