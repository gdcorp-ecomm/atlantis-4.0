using System;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestSiteContext : ProviderBase, ISiteContext
  {
    public int ContextId { get { return 1; } }

    public string StyleId { get { return "1"; } }

    public int PrivateLabelId { get { return 1; } }

    public string ProgId { get { return string.Empty; } }

    public int PageCount { get { return 1; } }

    public string Pathway { get { return Guid.NewGuid().ToString(); } }

    public string CI { get { return string.Empty; } }

    public string ISC { get { return string.Empty; } }

    public bool IsRequestInternal { get { return true; } }

    public ServerLocationType ServerLocation { get { return ServerLocationType.Dev; } }

    public IManagerContext Manager { get { return null; } }

    public TestSiteContext(IProviderContainer container) : base(container)
    {
    }

    public HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      return new HttpCookie("empty");
    }

    public HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      return new HttpCookie("empty");
    }
  }
}
