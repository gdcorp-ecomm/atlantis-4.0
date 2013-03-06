using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockSiteContext : ProviderBase, ISiteContext
  {
    public MockSiteContext(IProviderContainer container)
      : base(container)
    {
    }

    public int ContextId 
    {
      get
      {
        return KnownPrivateLabelIds.GetContextId(PrivateLabelId);
      }
    }


    public string StyleId 
    {
      get
      {
        string result = "0";
        switch (PrivateLabelId)
        {
          case KnownPrivateLabelIds.GoDaddy:
            result = "1";
            break;
          case KnownPrivateLabelIds.BlueRazor:
            result = "2";
            break;
          case KnownPrivateLabelIds.WildWestDomains:
            result = "1387";
            break;
        }
        return result;
      }
    }

    public int PrivateLabelId
    {
      get
      {
        int result = KnownPrivateLabelIds.GoDaddy;

        if (Manager.IsManager)
        {
          result = Manager.ManagerPrivateLabelId;
        }
        else if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] != null)
          {
            result = (int)HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId];
          }
        }
        return result;
      }
    }

    public string ProgId
    {
      get
      {
        return DataCache.DataCache.GetProgID(PrivateLabelId);
      }
    }

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
          object pageCount = HttpContext.Current.Items[MockSiteContextSettings.PageCount];
          if ((pageCount != null) && (pageCount is int))
          {
            result = (int)pageCount;
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
