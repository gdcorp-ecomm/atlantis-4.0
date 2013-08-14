using Atlantis.Framework.DataCacheService;
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
        int result;

        if ((IsManagerAvailable) && (Manager.IsManager))
        {
          result = Manager.ManagerPrivateLabelId;
        }
        else
        {
          result = Container.GetData(MockSiteContextSettings.PrivateLabelId, KnownPrivateLabelIds.GoDaddy);
        }
        return result;
      }
    }

    public string ProgId
    {
      get
      {
        string progId;

        using (GdDataCacheOutOfProcess dataCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          progId = dataCache.GetProgId(PrivateLabelId);
        }

        return progId;
      }
    }

    public HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      var result = new HttpCookie(cookieName) {Expires = expiration};
      return result;
    }

    public HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      var result = new HttpCookie(cookieName);
      return result;
    }

    public int PageCount
    {
      get
      {
        return Container.GetData(MockSiteContextSettings.PageCount, 0);
      }
    }

    public string Pathway
    {
      get
      {
        return Container.GetData(MockSiteContextSettings.Pathway, string.Empty);
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
        return Container.GetData(MockSiteContextSettings.IsRequestInternal, false);
      }
    }

    public ServerLocationType ServerLocation
    {
      get
      {
        return Container.GetData(MockSiteContextSettings.ServerLocation, ServerLocationType.Dev);
      }
    }

    private bool IsManagerAvailable
    {
      get { return Container.CanResolve<IManagerContext>(); }
    }

    public IManagerContext Manager
    {
      get 
      {
        if (IsManagerAvailable)
        {
          return Container.Resolve<IManagerContext>();
        }
        return null;
      }
    }
  }
}
