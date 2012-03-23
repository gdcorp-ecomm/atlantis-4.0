using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthCaptchaRequired.Interface
{
  [Serializable]
  public class AuthCaptchaRequiredResponseData : IResponseData
  {
    private AtlantisException _aex;

    public bool IsCaptchaRequired { get; private set; }

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public long StatusCode { get; set; }

    public AuthCaptchaRequiredResponseData(bool isCaptchaRequired, HashSet<int> validationCodes, long statusCode, string statusMessage)
    {
      IsCaptchaRequired = isCaptchaRequired;
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      StatusCode = statusCode;
    }

    public AuthCaptchaRequiredResponseData(AtlantisException ex)
    {
      _aex = ex;
    }

    public AuthCaptchaRequiredResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthCaptchaRequiredResponseData", ex.Message, string.Empty, ex);
    }

    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(bool));
        xmlSerializer.Serialize(writer, IsCaptchaRequired);
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
