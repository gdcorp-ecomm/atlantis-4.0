using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthSendPasswordResetEmail.Interface
{
  public class AuthSendPasswordResetEmailResponseData : IResponseData
  {
    private AtlantisException _aex;
    private string _resultXml = string.Empty;

    public long StatusCode { get; private set; }    
    public HashSet<int> ValidationCodes { get; private set; }
    public string StatusMessage { get; private set; }
    
    public AuthSendPasswordResetEmailResponseData(long statusCode, HashSet<int> validationErrors, string statusMessage, string resultXml)
    {
      StatusCode = statusCode;
      _resultXml = resultXml;
      ValidationCodes = validationErrors;
      StatusMessage = statusMessage;
    }

    public AuthSendPasswordResetEmailResponseData(AtlantisException aex)
    {
      _aex = aex;
    }

    public AuthSendPasswordResetEmailResponseData(Exception ex, RequestData requestData)
    {
      _aex = new AtlantisException(requestData, "AuthSendPasswordResetEmailResponseData", ex.Message, "ShopperID: " + requestData.ShopperID);
    }

    public AuthSendPasswordResetEmailResponseData(long statusCode, string resultXml)
    {
      StatusCode = statusCode;
      _resultXml = resultXml;
    }

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
