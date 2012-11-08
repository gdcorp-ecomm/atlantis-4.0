using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Testing.MockProviders
{
  public abstract class MockSiteContextBase : ProviderBase, ISiteContext
  {
    public MockSiteContextBase(IProviderContainer container)
      : base(container)
    {
    }

    public abstract int ContextId { get; }
    public abstract string StyleId {get;}
    public abstract int PrivateLabelId {get;}
    public abstract string ProgId {get;}

    public System.Web.HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      HttpCookie result = new System.Web.HttpCookie(cookieName);
      result.Expires = expiration;
      return result;
    }

    public System.Web.HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      HttpCookie result = new System.Web.HttpCookie(cookieName);
      return result;
    }

    public int PageCount
    {
      get 
      {
        int result = 0;
        if (HttpContext.Current != null)
        {
          string pageCount = HttpContext.Current.Items[MockSiteContextSettings.PageCount] as string;
          if ((!string.IsNullOrEmpty(pageCount)) && (!int.TryParse(pageCount, out result)))
          {
            result = 0;
          }
        }
        return result;
      }
    }

    public string Pathway
    {
      get 
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          string pathway = HttpContext.Current.Items[MockSiteContextSettings.Pathway] as string;
          if (pathway != null)
          {
            result = pathway;
          }
        }
        return result;
      }
    }

    public string CI
    {
      get 
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.QueryString["ci"] ?? string.Empty;
        }
        return result;
      }
    }

    public string ISC
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.QueryString["isc"] ?? string.Empty;
        }
        return result;
      }
    }

    public bool IsRequestInternal
    {
      get 
      {
        bool result = false;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal] != null)
          {
            result = (bool)HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal];
          }
        }
        return result;
      }
    }

    public ServerLocationType ServerLocation
    {
      get
      {
        ServerLocationType result = ServerLocationType.Dev;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Items[MockSiteContextSettings.IsRequestInternal] != null)
          {
            result = (ServerLocationType)HttpContext.Current.Items[MockSiteContextSettings.ServerLocation];
          }
        }
        return result;
      }
    }

    public IManagerContext Manager
    {
      get 
      {
        return Container.Resolve<IManagerContext>();
      }
    }
  }
}
