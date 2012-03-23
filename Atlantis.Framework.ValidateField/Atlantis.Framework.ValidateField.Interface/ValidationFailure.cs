using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidationFailure
  {
    public int FailureCode { get; private set; }
    public string Description { get; private set; }

    public ValidationFailure(int failureCode, string description)
    {
      FailureCode = failureCode;
      Description = description;
    }
  }
}
