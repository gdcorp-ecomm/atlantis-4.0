using System;
using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  public class SiteAdminContextProvider : ProviderBase, ISiteContext
  {
    public SiteAdminContextProvider(IProviderContainer container)
      : base(container)
    { }

    #region ISiteContext Members

    public int ContextId
    {
      get { return ContextIds.SiteAdmin; }
    }

    public string StyleId
    {
      get { return string.Empty; }
    }

    public int PrivateLabelId
    {
      get { return 0; }
    }

    public string ProgId
    {
      get { return string.Empty; }
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

    private string _crossDomainCookieDomain = null;
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

    private const string COOKIE_PAGECOUNT = "pagecount";
    private int? _pageCount;
    public int PageCount
    {
      get
      {
        if (_pageCount == null)
        {
          _pageCount = 0;
          HttpCookie pageCountCookie = HttpContext.Current.Request.Cookies[COOKIE_PAGECOUNT];
          if (pageCountCookie != null)
          {
            int pageCount;
            if (Int32.TryParse(pageCountCookie.Value, out pageCount))
              _pageCount = pageCount;
          }
        }

        return _pageCount.Value;
      }
    }

    private const string COOKIE_PATHWAY = "pathway";
    private string _pathway = null;
    public string Pathway
    {
      get
      {
        if (_pathway == null)
        {
          HttpCookie pathwayCookie = HttpContext.Current.Request.Cookies[COOKIE_PATHWAY];
          if (pathwayCookie != null)
            _pathway = pathwayCookie.Value ?? string.Empty;
          else
            _pathway = string.Empty;
        }

        return _pathway;
      }
    }

    private const string PARAM_CI = "ci";
    private string _ci = null;
    public string CI
    {
      get
      {
        if (_ci == null)
        {
          string ci = HttpContext.Current.Request[PARAM_CI] ?? string.Empty;
          ci = HttpUtility.HtmlEncode(ci.Trim());
          if (ci.Contains(","))
            ci = ci.Substring(0, ci.IndexOf(','));
          _ci = ci;
        }

        return _ci;
      }
    }

    public string ISC
    {
      get { return string.Empty; }
    }

    private bool? _isRequestInternal = null;
    public virtual bool IsRequestInternal
    {
      get
      {
        if (_isRequestInternal == null)
        {
          _isRequestInternal = RequestHelper.IsRequestInternal();
        }
        return (bool)_isRequestInternal;
      }
    }

    private ServerLocationType _serverLocation = ServerLocationType.Undetermined;
    public ServerLocationType ServerLocation
    {
      get
      {
        if (_serverLocation == ServerLocationType.Undetermined)
        {
          _serverLocation = RequestHelper.GetServerLocation(IsRequestInternal);
        }
        return _serverLocation;
      }
    }

    private IManagerContext _managerContext = null;
    public IManagerContext Manager
    {
      get
      {
        if (_managerContext == null)
        {
          _managerContext = Container.Resolve<IManagerContext>();
        }
        return _managerContext;
      }
    }

    #endregion
  }
}
