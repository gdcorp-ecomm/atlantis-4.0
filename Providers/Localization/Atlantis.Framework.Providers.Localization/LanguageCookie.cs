using Atlantis.Framework.Interface;
using System;
using System.Globalization;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  public class LanguageCookie
  {
    readonly IProviderContainer _container;
    readonly Lazy<string> _cookieName;
    readonly Lazy<HttpCookie> _requestCookie;
    readonly Lazy<ISiteContext> _siteContext;

    internal LanguageCookie(IProviderContainer container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _cookieName = new Lazy<string>(LoadCookieName);
      _requestCookie = new Lazy<HttpCookie>(LoadRequestCookie);
    }

    private string LoadCookieName()
    {
      return "language" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
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
