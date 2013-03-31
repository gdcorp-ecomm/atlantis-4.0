using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  internal class CountrySiteCookie
  {
    IProviderContainer _container;
    Lazy<string> _cookieName;
    Lazy<HttpCookie> _requestCookie;
    Lazy<ISiteContext> _siteContext;

    internal CountrySiteCookie(IProviderContainer container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => { return _container.Resolve<ISiteContext>(); });
      _cookieName = new Lazy<string>(() => { return LoadCookieName(); });
      _requestCookie = new Lazy<HttpCookie>(() => { return LoadRequestCookie(); });
    }

    private string LoadCookieName()
    {
      return "countrysite" + _siteContext.Value.PrivateLabelId.ToString();
    }

    private HttpCookie LoadRequestCookie()
    {
      return HttpContext.Current.Request.Cookies[_cookieName.Value];
    }

    public bool HasValue
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false;
        }

        return _requestCookie.Value != null;
      }
    }

    public string Value
    {
      get
      {
        string result = string.Empty;
        if ((HttpContext.Current != null) && (_requestCookie.Value != null))
        {
          result = _requestCookie.Value.Value;
        }
        return result;
      }
      set
      {
        if (HttpContext.Current != null)
        {
          HttpCookie setCookie = _siteContext.Value.NewCrossDomainCookie(_cookieName.Value, DateTime.Now.AddYears(1));
          setCookie.Value = value;
          HttpContext.Current.Response.Cookies.Set(setCookie);
        }
      }
    }
  }
}
