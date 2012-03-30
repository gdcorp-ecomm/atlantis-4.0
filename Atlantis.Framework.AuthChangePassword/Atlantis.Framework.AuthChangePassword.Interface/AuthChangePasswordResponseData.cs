using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.AuthChangePassword.Interface
{
  public class AuthChangePasswordResponseData : IResponseData
  {
    private AtlantisException _ex;

    public List<ValidationFailure> RegexErrors { get; private set; }
    
    public HashSet<int> ValidationCodes { get; private set; }

    public long StatusCode { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthChangePasswordResponseData(long statusCode, string statusMessage, HashSet<int> validationCodes, List<ValidationFailure> regexErrors)
    {
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? string.Empty;
      RegexErrors = new List<ValidationFailure>();
      RegexErrors = regexErrors;
    }

    public AuthChangePasswordResponseData(RequestData requestData, Exception ex)
    {
      _ex = new AtlantisException(
        requestData, "AuthChangePasswordResponseData", "Exception when Changing Password", ex.Message);
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
