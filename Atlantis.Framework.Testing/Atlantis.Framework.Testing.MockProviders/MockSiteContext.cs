using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockSiteContext : MockProviderBase, ISiteContext
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

        if ((IsManagerAvailable) && (Manager.IsManager))
        {
          result = Manager.ManagerPrivateLabelId;
        }
        else
        {
          object mockPrivateLabelId = GetMockSetting(MockSiteContextSettings.PrivateLabelId);
          if (mockPrivateLabelId != null)
          {
            result = Convert.ToInt32(mockPrivateLabelId);
          }
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

        object mockPageCount = GetMockSetting(MockSiteContextSettings.PageCount);
        if (mockPageCount != null)
        {
          result = Convert.ToInt32(mockPageCount);
        }

        return result;
      }
    }

    public string Pathway
    {
      get 
      {
        string result = string.Empty;

        string mockPathway = GetMockSetting(MockSiteContextSettings.Pathway) as string;
        if (mockPathway != null)
        {
          result = mockPathway;
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

        object mockIsRequestInternal = GetMockSetting(MockSiteContextSettings.IsRequestInternal);
        if (mockIsRequestInternal != null)
        {
          result = Convert.ToBoolean(mockIsRequestInternal);
        }

        return result;
      }
    }

    public ServerLocationType ServerLocation
    {
      get
      {
        ServerLocationType result = ServerLocationType.Dev;

        object mockServerLocation = GetMockSetting(MockSiteContextSettings.ServerLocation);
        if (mockServerLocation != null)
        {
          result = (ServerLocationType)mockServerLocation;
        }

        return result;
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
        return Container.Resolve<IManagerContext>();
      }
    }
  }
}
