using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthNamespace.Interface
{
  public class AuthNamespaceResponseData : IResponseData
  {
    private readonly AtlantisException m_ex;

    private AuthNamespaceResponseData( int statusCode, string statusMessage )
    {
      ShopperId = String.Empty;
      Email = String.Empty;
      StatusCodes = new HashSet<int> { statusCode };
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthNamespaceResponseData( string shopperId, string email, IEnumerable<int> statusCodes, string statusMessage )
    {
      ShopperId = shopperId;
      Email = email;
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthNamespaceResponseData( AtlantisException ex )
      : this( AuthNamespaceStatusCodes.Error, null )
    {
      m_ex = ex;
    }

    public AuthNamespaceResponseData( RequestData requestData, Exception ex )
      : this( AuthNamespaceStatusCodes.Error, null )
    {
      m_ex = new AtlantisException( requestData, "AuthNamespaceResponseData",
        "Exception when Lookup up Namespace", String.Empty, ex );
    }

    public string ShopperId { get; private set; }

    public string Email { get; private set; }

    public HashSet<int> StatusCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthNamespaceStatusCodes.Success )); }
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
