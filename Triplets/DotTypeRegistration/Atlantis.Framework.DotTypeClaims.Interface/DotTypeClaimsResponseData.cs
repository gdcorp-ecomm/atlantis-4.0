using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Dictionary<string, IEnumerable<string>> _claimsXmlByDomain;

    private DotTypeClaimsResponseData()
    {
      _noticeXmlByDomain = new Dictionary<string, string>();
      _claimsXmlByDomain = new Dictionary<string, IEnumerable<string>>();
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
        _claimsXmlByDomain = new Dictionary<string, IEnumerable<string>>();

        XElement strResponse = XElement.Parse(responseXml);

        bool.TryParse(strResponse.Attribute("success").Value, out _isSuccess);

        if (_isSuccess)
        {
          var domains = strResponse.Descendants("domain");
          foreach (var domain in domains)
          {
            if (!string.IsNullOrEmpty(domain.Value))
            {
              var noticeElement = XElement.Parse(domain.Value);
              foreach (XElement xelement in noticeElement.DescendantsAndSelf())
              {
                // Stripping the namespace by setting the name of the element to it's localname only
                xelement.Name = xelement.Name.LocalName;

                // replacing all attributes with attributes that are not namespaces and their names are set to only the localname
                xelement.ReplaceAttributes((from xattrib in xelement.Attributes().Where(xattr => !xattr.IsNamespaceDeclaration) select new XAttribute(xattrib.Name.LocalName, xattrib.Value)));
              }

              _noticeXmlByDomain[domain.Attribute("name").Value] = noticeElement.ToString();

              IList<string> claims = new List<string>();
              var claimElements = noticeElement.Descendants("claim");
              foreach (XElement claimElement in claimElements)
              {
                claims.Add(claimElement.ToString());
              }

              _claimsXmlByDomain[domain.Attribute("name").Value] = claims;
            }
            else
            {
              _noticeXmlByDomain[domain.Attribute("name").Value] = string.Empty;
              _claimsXmlByDomain[domain.Attribute("name").Value] = new List<string>();
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
