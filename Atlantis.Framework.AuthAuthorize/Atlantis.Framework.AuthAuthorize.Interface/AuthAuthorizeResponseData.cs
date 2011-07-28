using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthAuthorize.Interface
{
  public class AuthAuthorizeResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    public string ResponseXml { get; set; }
    public HashSet<int> StatusCodes { get; private set; }
    public string StatusMessage { get; private set; }

    public AuthAuthorizeResponseData( string responseXml, IEnumerable<int> statusCodes, string statusMessage )
    {
      ResponseXml = responseXml;
      StatusCodes = new HashSet<int>( statusCodes );
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthAuthorizeResponseData( AtlantisException ex )
    {
      _exception = ex;
      StatusCodes = new HashSet<int>();
      StatusCodes.Add(AuthAuthorizeStatusCodes.Error);
      StatusMessage = "Unknown error.";
    }

    public bool IsSuccess
    {
      get { return (StatusCodes.Count == 1) && (StatusCodes.Contains( AuthAuthorizeStatusCodes.Success )); }
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
