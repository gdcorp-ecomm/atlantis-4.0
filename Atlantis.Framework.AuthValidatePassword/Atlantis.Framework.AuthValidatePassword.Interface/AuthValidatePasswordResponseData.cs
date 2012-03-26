using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthValidatePassword.Interface
{
  public class AuthValidatePasswordResponseData : IResponseData
  {
    public bool IsPasswordValid { get; private set; }

    public long StatusCode { get; private set; }

    private AtlantisException _aex;

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthValidatePasswordResponseData(bool isPasswordValid, HashSet<int> validationCodes, long statusCode, string statusMessage)
    {
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      IsPasswordValid = isPasswordValid;
      StatusCode = statusCode;
    }

    public AuthValidatePasswordResponseData(AtlantisException ex)
    {
      _aex = ex;
    }

    public AuthValidatePasswordResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthValidatePasswordResponseData", ex.Message, string.Empty, ex);
    }

    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(bool));
        xmlSerializer.Serialize(writer, IsPasswordValid);
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
