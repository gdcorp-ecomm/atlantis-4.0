using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EmailMobileTemplate.Interface
{
  public class EmailMobileTemplateResponseData : IResponseData
  {
    private const string DEFAULT_HTML = "Email Template Not Found.";

    private AtlantisException AtlEx { get; set; }

    public string MobileTemplateHtml { get; private set; }

    public bool IsSuccess { get; private set; }

    public EmailMobileTemplateResponseData(string html)
    {
      if(!string.IsNullOrEmpty(html))
      {
        MobileTemplateHtml = html;
        IsSuccess = true;
      }
      else
      {
        MobileTemplateHtml = DEFAULT_HTML;
        IsSuccess = false;
      }
    }

    public EmailMobileTemplateResponseData(RequestData requestData, Exception ex)
    {
      MobileTemplateHtml = DEFAULT_HTML;
      AtlEx = new AtlantisException(requestData, "EmailMobileTemplateRequest", ex.Message, ex.Data == null ? string.Empty : ex.Data.ToString());
      IsSuccess = false;
    }

    public string ToXML()
    {
      return MobileTemplateHtml;
    }

    public AtlantisException GetException()
    {
      return AtlEx;
    }
  }
}
