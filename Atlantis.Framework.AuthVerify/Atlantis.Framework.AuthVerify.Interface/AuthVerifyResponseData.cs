using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthVerify.Interface
{
  public class AuthVerifyResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    private AuthVerifyResponseData( int statusCode, string statusMessage )
    {
      LoginKey = String.Empty;
      ValidationSource = String.Empty;
      StatusCodes = new HashSet<int> { statusCode };
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthVerifyResponseData( string loginKey, string validationSource, IEnumerable<int> statusCodes, string statusMessage )
    {
      LoginKey = loginKey;
      ValidationSource = validationSource;
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthVerifyResponseData( AtlantisException ex )
      : this( AuthVerifyStatusCodes.Error, null )
    {
      m_ex = ex;
    }

    public AuthVerifyResponseData( RequestData requestData, Exception ex )
      : this( AuthVerifyStatusCodes.Error, null )
    {
      m_ex = new AtlantisException( requestData, "AuthVerifyResponseData",
        "Exception when Verifying", String.Empty, ex );
    }

    public string LoginKey { get; private set; }

    public string ValidationSource { get; private set; }

    public HashSet<int> StatusCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthVerifyStatusCodes.Success )); }
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
