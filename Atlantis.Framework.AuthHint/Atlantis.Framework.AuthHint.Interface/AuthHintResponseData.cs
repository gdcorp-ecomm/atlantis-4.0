using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthHint.Interface
{
  public class AuthHintResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    private AuthHintResponseData( int statusCode, string statusMessage )
    {
      Hint = String.Empty;
      StatusCodes = new HashSet<int> { statusCode };
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthHintResponseData( string hint, IEnumerable<int> statusCodes, string statusMessage )
    {
      Hint = hint;
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthHintResponseData( AtlantisException ex )
      : this( AuthHintStatusCodes.Error, null )
    {
      m_ex = ex;
    }

    public AuthHintResponseData( RequestData requestData, Exception ex )
      : this( AuthHintStatusCodes.Error, null )
    {
      m_ex = new AtlantisException( requestData, "AuthHintResponseData",
        "Exception when Getting Hint", String.Empty, ex );
    }

    public string Hint { get; private set; }

    public HashSet<int> StatusCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthHintStatusCodes.Success )); }
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
