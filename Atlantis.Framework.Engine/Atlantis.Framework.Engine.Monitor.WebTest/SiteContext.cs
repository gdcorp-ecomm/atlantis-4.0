using System;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public class SiteContext : ProviderBase, ISiteContext
  {
    public SiteContext(IProviderContainer container) : base(container) { }

    public int ContextId
    {
      get { throw new NotImplementedException(); }
    }

    public string StyleId
    {
      get { throw new NotImplementedException(); }
    }

    public int PrivateLabelId
    {
      get { throw new NotImplementedException(); }
    }

    public string ProgId
    {
      get { throw new NotImplementedException(); }
    }

    public HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      throw new NotImplementedException();
    }

    public HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      throw new NotImplementedException();
    }

    public int PageCount
    {
      get { throw new NotImplementedException(); }
    }

    public string Pathway
    {
      get { throw new NotImplementedException(); }
    }

    public string CI
    {
      get { throw new NotImplementedException(); }
    }

    public string ISC
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsRequestInternal
    {
      get { return true; }
    }

    public ServerLocationType ServerLocation
    {
      get { throw new NotImplementedException(); }
    }

    public IManagerContext Manager
    {
      get { throw new NotImplementedException(); }
    }
  }
}