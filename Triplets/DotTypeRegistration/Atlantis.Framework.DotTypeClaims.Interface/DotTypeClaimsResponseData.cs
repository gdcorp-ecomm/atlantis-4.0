using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsResponseData : IResponseData
  {
    public static DotTypeClaimsResponseData Empty { get; private set; }

    static DotTypeClaimsResponseData()
    {
      Empty = new DotTypeClaimsResponseData();
    }

    private string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;

    public readonly string NoticeXml;
    public readonly string HtmlData;

    private DotTypeClaimsResponseData()
    {
    }

    public static DotTypeClaimsResponseData FromResponseXml(string responseXml)
    {
      if (!string.IsNullOrEmpty(responseXml))
      {
        return new DotTypeClaimsResponseData(responseXml);
      }
      
      return Empty;
    }

    private DotTypeClaimsResponseData(string responseXml)
    {
      try
      {
        _responseXml = responseXml;

        if (!string.IsNullOrEmpty(responseXml))
        {
          XElement claimElement = XElement.Parse(responseXml);
          var claim = claimElement.Name.LocalName;
          if (claim.Equals("claim"))
          {
            var noticeXmlAttr = claimElement.Attribute("tmchdata");
            var htmlDataAttr = claimElement.Attribute("html");

            if (noticeXmlAttr == null || htmlDataAttr == null)
            {
              _isSuccess = false;

              const string message = "Xml with invalid NoticeXml, HtmlData";
              var xmlHeaderException = new AtlantisException("DotTypeClaimsResponseData", "0", message, responseXml, null, null);
              throw xmlHeaderException;
            }
            else
            {
              _isSuccess = true;

              if (!string.IsNullOrEmpty(noticeXmlAttr.Value))
              {
                NoticeXml = noticeXmlAttr.Value;
              }

              if (!string.IsNullOrEmpty(htmlDataAttr.Value))
              {
                HtmlData = htmlDataAttr.Value;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        _isSuccess = false;

        var exception = new AtlantisException("DotTypeClaimsResponseData.FromResponseXml", "0", ex.Message + ex.StackTrace, responseXml, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
    }

    public static DotTypeClaimsResponseData FromException(AtlantisException exception)
    {
      return new DotTypeClaimsResponseData(exception);
    }

    private DotTypeClaimsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }
  }
}
