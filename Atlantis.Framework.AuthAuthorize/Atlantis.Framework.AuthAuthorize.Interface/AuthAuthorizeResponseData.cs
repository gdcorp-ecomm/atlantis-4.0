using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthAuthorize.Interface
{
  public class AuthAuthorizeResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    
    public string ResponseXml { get; set; }

    public HashSet<int> ValidationCodes { get; private set; }

    public int StatusCode { get; private set; }
    
    public string StatusMessage { get; private set; }

    public AuthAuthorizeResponseData(string responseXml, int statusCode, HashSet<int> validationCodes, string statusMessage)
    {
      ResponseXml = responseXml;
      StatusCode = statusCode;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage ?? String.Empty;
    }

    public AuthAuthorizeResponseData(AtlantisException ex)
    {
      _exception = ex;
      ValidationCodes = new HashSet<int>();
      StatusCode = AuthAuthorizeStatusCodes.Error;
      StatusMessage = "Unknown error.";
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
