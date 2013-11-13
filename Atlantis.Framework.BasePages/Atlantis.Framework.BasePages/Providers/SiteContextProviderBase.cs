using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.BasePages.Providers
{
  public abstract class SiteContextProviderBase : ProviderBase, ISiteContext
  {
    #region ISiteContext Members

    public abstract int ContextId { get; }
    public abstract string StyleId { get; }
    public abstract int PrivateLabelId { get; }
    public abstract string ProgId { get; }

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
          string result = RequestHelper.SafeUrlHost();
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
    private string _pathway;
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
    private string _ci;
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

    private const string PARAM_ISC = "isc";
    private string _isc;
    public string ISC
    {
      get
      {
        // TODO: This structure does not support marketings consistent changes
        // to ISC defaults or special sales that default an ISC code to a visitor
        // without one.  But ISC is needed by the base page for CI tracking.
        if (_isc == null)
        {
          string isc = HttpContext.Current.Request[PARAM_ISC] ?? string.Empty;
          if (Manager.IsManager)
          {
            isc = HttpContext.Current.Request.QueryString[PARAM_ISC] ?? string.Empty;
          }

          if (isc.Contains(","))
            isc = isc.Substring(0, isc.IndexOf(','));
          if (isc.Length > 10)
            isc = isc.Substring(0, 10);
          _isc = isc;
        }

        return _isc;
      }
    }

    private bool? _isRequestInternal;
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

    private IManagerContext _managerContext;
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

    protected SiteContextProviderBase(IProviderContainer providerContainer)
      : base(providerContainer)
    {
    }
  }
}
