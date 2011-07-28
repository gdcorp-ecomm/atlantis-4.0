using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthToken.Interface
{
  public class AuthTokenResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    private AuthTokenResponseData( int statusCode, string statusMessage )
    {
      AuthToken = String.Empty;
      StatusCodes = new HashSet<int> { statusCode };
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthTokenResponseData( string authToken, IEnumerable<int> statusCodes, string statusMessage )
    {
      AuthToken = authToken;
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthTokenResponseData( AtlantisException ex )
      : this( AuthTokenStatusCodes.Error, null )
    {
      m_ex = ex;
    }

    public AuthTokenResponseData( RequestData requestData, Exception ex )
      : this( AuthTokenStatusCodes.Error, null )
    {
      m_ex = new AtlantisException( requestData, "AuthTokenResponseData",
        "Exception when Getting Auth Token", String.Empty, ex );
    }

    public string AuthToken { get; private set; }

    public HashSet<int> StatusCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthTokenStatusCodes.Success )); }
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
