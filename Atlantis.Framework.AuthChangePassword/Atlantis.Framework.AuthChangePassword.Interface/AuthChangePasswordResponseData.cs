using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthChangePassword.Interface
{
  public class AuthChangePasswordResponseData : IResponseData
  {
    private AtlantisException _ex;
    private HashSet<int> _statusCodes = new HashSet<int>();
    private string _statusMessage = null;

    public HashSet<int> StatusCodes
    {
      get { return _statusCodes; }
    }

    public string StatusMessage
    {
      get
      {
        string result = string.Empty;
        if (_statusMessage != null)
        {
          result = _statusMessage;
        }
        return result;
      }
    }

    public bool IsSuccess
    {
      get { return (_statusCodes.Count == 1) && (_statusCodes.Contains(AuthChangePasswordStatusCodes.Success)); }
    }

    public AuthChangePasswordResponseData(IEnumerable<int> statusCodes, string statusMessage)
    {
      _statusCodes = new HashSet<int>(statusCodes);
      _statusMessage = statusMessage;
    }

    public AuthChangePasswordResponseData(AtlantisException ex)
    {
      _ex = ex;
      _statusCodes.Add(AuthChangePasswordStatusCodes.Error);
    }

    public AuthChangePasswordResponseData(RequestData requestData, Exception ex)
    {
      _ex = new AtlantisException(
        requestData, "AuthChangePasswordResponseData", "Exception when Changing Password", string.Empty);
      _statusCodes.Add(AuthChangePasswordStatusCodes.Error);
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
