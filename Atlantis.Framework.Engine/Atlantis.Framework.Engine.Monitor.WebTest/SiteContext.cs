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
      HttpCookie result = new HttpCookie(cookieName);
      result.Expires = expiration;
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    public HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      HttpCookie result = new HttpCookie(cookieName);
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    private string _crossDomainCookieDomain;
    private string CrossDomainCookieDomain
    {
      get
      {
        if (_crossDomainCookieDomain == null)
        {
          string result = HttpContext.Current.Request.Url.Host;
          if (result == "localhost")
            result = null;
          else if (result.Contains("."))
          {
            string[] parts = result.Split('.');
            if (parts.Length > 2)
              result = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
          }
          _crossDomainCookieDomain = result;
        }
        return _crossDomainCookieDomain;
      }
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