using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ValidateInput.Interface
{
  public interface IValidateInputResult
  {
    bool IsSuccess { get; }
    IList<int> ErrorCodes { get; }
  }
}