using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.IO;
using System.Xml.Serialization;
namespace Atlantis.Framework.AuthTwoFactorValidateToken.Interface
{
  public class AuthTwoFactorValidateTokenResponseData:IResponseData
  {
    public bool IsTokenValid { get; private set; }

    public long StatusCode { get; private set; }

    private AtlantisException _aex;

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthTwoFactorValidateTokenResponseData(bool tokenIsValid, HashSet<int> validationCodes, long statusCode, string statusMessage)
    {
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      IsTokenValid = tokenIsValid;
      StatusCode = statusCode;
    }

    public AuthTwoFactorValidateTokenResponseData(AtlantisException ex)
    {
      _aex = ex;
    }

    public AuthTwoFactorValidateTokenResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthTwoFactorValidateTokenResponseData", ex.Message, string.Empty, ex);
    }
    
    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(bool));
        xmlSerializer.Serialize(writer, IsTokenValid);
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
