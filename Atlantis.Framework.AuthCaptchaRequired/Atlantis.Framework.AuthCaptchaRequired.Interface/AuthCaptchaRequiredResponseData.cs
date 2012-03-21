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
    public bool IsSuccess { get; set; }
    private AtlantisException _aex;

    public bool IsCaptchaRequired { get; private set; }

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public AuthCaptchaRequiredResponseData(bool isCaptchaRequired, HashSet<int> validationCodes, string statusMessage)
    {
      IsCaptchaRequired = isCaptchaRequired;
      ValidationCodes = validationCodes;
      IsSuccess = validationCodes.Count == 0;
      StatusMessage = statusMessage;  
    }

    public AuthCaptchaRequiredResponseData(AtlantisException ex)
    {
      _aex = ex;
      IsSuccess = false;
    }

    public AuthCaptchaRequiredResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthCaptchaRequiredResponseData", ex.Message, string.Empty, ex);
      IsSuccess = false;
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
