using System.Collections.Generic;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputResult
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