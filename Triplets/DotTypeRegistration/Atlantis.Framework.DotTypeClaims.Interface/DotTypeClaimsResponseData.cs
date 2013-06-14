using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsResponseData : IResponseData
  {
    public static DotTypeClaimsResponseData Empty { get; private set; }

    static DotTypeClaimsResponseData()
    {
      Empty = new DotTypeClaimsResponseData();
    }

    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;

    private readonly IDotTypeClaimsSchema _dotTypeClaimsSchema;
    private readonly Dictionary<string, string> _noticeXmlByDomain;
    private readonly Dictionary<string, string> _claimsXmlByDomain;

    private DotTypeClaimsResponseData()
    {
      _noticeXmlByDomain = new Dictionary<string, string>();
      _claimsXmlByDomain = new Dictionary<string, string>();
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

        _noticeXmlByDomain = new Dictionary<string, string>();
        _claimsXmlByDomain = new Dictionary<string, string>();

        XElement strResponse = XElement.Parse(responseXml);

        bool.TryParse(strResponse.Attribute("success").Value, out _isSuccess);

        if (_isSuccess)
        {
          var domains = strResponse.Descendants("domain");
          foreach (var domain in domains)
          {
            var noticeElement = domain.Element("notice");
            if (noticeElement != null)
            {
              _noticeXmlByDomain[domain.Attribute("name").Value] = noticeElement.ToString();

              var claimsElement = noticeElement.Element("claims");
              if (claimsElement != null)
              {
                _claimsXmlByDomain[domain.Attribute("name").Value] = claimsElement.ToString();
              }
            }
          }
          _dotTypeClaimsSchema = new DotTypeClaimsSchema(_noticeXmlByDomain, _claimsXmlByDomain);
        }
      }
      catch (Exception ex)
      {
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

    public IDotTypeClaimsSchema DotTypeClaims
    {
      get { return _dotTypeClaimsSchema; }  
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
