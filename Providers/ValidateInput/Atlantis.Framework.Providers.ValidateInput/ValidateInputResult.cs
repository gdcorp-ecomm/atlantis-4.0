using System.Collections.Generic;
using Atlantis.Framework.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput
{
  public class ValidateInputResult : IValidateInputResult
  {
    private static ValidateInputResult _failureResult;
    private static ValidateInputResult FailureResult
    {
      get { return _failureResult ?? (_failureResult = new ValidateInputResult(false, new List<int>())); }
    }

    public ValidateInputResult(bool isSuccess, IList<int> errorCodes)
    {
      _isSuccess = isSuccess;
      _errorCodes = errorCodes;
    }

    public static ValidateInputResult GetValidateInputResult(Framework.ValidateInput.Interface.ValidateInputResult result)
    {
      return new ValidateInputResult(result.IsSuccess, result.ErrorCodes);
    }

    public static ValidateInputResult CreateFailureResult()
    {
      return FailureResult;
    }

    private readonly bool _isSuccess;
    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    private IList<int> _errorCodes;
    public IList<int> ErrorCodes
    {
      get { return _errorCodes ?? (_errorCodes = new List<int>()); }
    }
  }
}
