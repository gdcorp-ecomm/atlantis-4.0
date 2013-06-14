using System;
using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsSchema : IDotTypeClaimsSchema
  {
    private readonly Dictionary<string, string> _noticeXmlByDomain;
    private readonly Dictionary<string, string> _claimsXmlByDomain;

    public bool TryGetNoticeXmlByDomain(string domain, out string noticeXml)
    {
      return _noticeXmlByDomain.TryGetValue(domain, out noticeXml);
    }

    public bool TryGetClaimsXmlByDomain(string domain, out string claimsXml)
    {
      return _claimsXmlByDomain.TryGetValue(domain, out claimsXml);
    }

    public DotTypeClaimsSchema(Dictionary<string, string> noticeXml, Dictionary<string, string> claimsXml)
    {
      _noticeXmlByDomain = noticeXml;
      _claimsXmlByDomain = claimsXml;
    }
  }
}
