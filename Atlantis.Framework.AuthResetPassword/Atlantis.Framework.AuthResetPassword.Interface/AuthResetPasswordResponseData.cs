using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public class AuthResetPasswordResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    private AuthResetPasswordResponseData( int statusCode, string statusMessage )
    {
      StatusCodes = new HashSet<int> { statusCode };
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthResetPasswordResponseData( IEnumerable<int> statusCodes, string statusMessage )
    {
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthResetPasswordResponseData( AtlantisException ex )
      : this( AuthResetPasswordStatusCodes.Error, null )
    {
      m_ex = ex;
    }

    public AuthResetPasswordResponseData( RequestData requestData, Exception ex )
      : this( AuthResetPasswordStatusCodes.Error, null )
    {
      m_ex = new AtlantisException( requestData, "AuthResetPasswordResponseData",
        "Exception when Resetting Password", String.Empty, ex );
    }

    public HashSet<int> StatusCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthResetPasswordStatusCodes.Success )); }
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return m_ex;
    }

    #endregion
  }
}
