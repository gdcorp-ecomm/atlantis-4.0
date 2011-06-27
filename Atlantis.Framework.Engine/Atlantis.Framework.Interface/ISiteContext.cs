using System;
using System.Web;

namespace Atlantis.Framework.Interface
{
  public interface ISiteContext
  {
    int ContextId { get; }
    string StyleId { get; }
    int PrivateLabelId { get; }
    string ProgId { get; }

    HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration);
    HttpCookie NewCrossDomainMemCookie(string cookieName);

    int PageCount { get; }
    string Pathway { get; }
    string CI { get; }
    string ISC { get; }

    bool IsRequestInternal { get; }
    ServerLocationType ServerLocation { get; }
    IManagerContext Manager { get; }
  }
}
