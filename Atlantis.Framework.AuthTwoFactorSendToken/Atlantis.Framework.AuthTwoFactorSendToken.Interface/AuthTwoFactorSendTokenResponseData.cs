using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorSendToken.Interface
{
  public class AuthTwoFactorSendTokenResponseData: IResponseData
  {
    public bool AuthTokenSent { get; set; }

    private AtlantisException _aex;

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public long StatusCode { get; set; }

    public AuthTwoFactorSendTokenResponseData(bool tokenSent, HashSet<int> validationCodes, long statusCode, string statusMessage)
    {
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      AuthTokenSent = tokenSent;
      StatusCode = statusCode;
    }

    public AuthTwoFactorSendTokenResponseData(AtlantisException ex)
    {
      _aex = ex;
    }

    public AuthTwoFactorSendTokenResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthTwoFactorSendTokenResponseData", ex.Message, string.Empty, ex);
    }
    
    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(bool));
        xmlSerializer.Serialize(writer, AuthTokenSent);
        xmlValue = writer.ToString();
      }
      catch (Exception) { }

      return xmlValue;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
