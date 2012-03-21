using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.IO;
using System.Xml.Serialization;

namespace Atlantis.Framework.AuthCaptchaRequired.Interface
{
  [Serializable]
  public class AuthCaptchaRequiredResponseData : IResponseData
  {
    public bool IsSuccess { get; set; }
    private AtlantisException _aex;

    private readonly bool _isRequired;
    public bool IsCaptchaRequired
    {
      get
      {
        return _isRequired;
      }
    }

    public AuthCaptchaRequiredResponseData(bool isCaptchaRequired)
    {
      _isRequired = isCaptchaRequired;
      IsSuccess = true;
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
      catch (Exception) {}

      return xmlValue;      
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
