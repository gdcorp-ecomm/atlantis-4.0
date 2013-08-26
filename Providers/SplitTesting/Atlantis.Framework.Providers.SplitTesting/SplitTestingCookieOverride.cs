using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.Interface;
using System.Linq;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingCookieOverride
  {
    readonly IProviderContainer _container;
    readonly Lazy<string> _cookieName;
    readonly Lazy<ISiteContext> _siteContext;

    internal SplitTestingCookieOverride(IProviderContainer container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _cookieName = new Lazy<string>(LoadCookieName);
    }

    private string LoadCookieName()
    {
      return "SplitTestingOverride" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
    }

    private HttpCookie RequestCookie
    {
      get
      {
        return HttpContext.Current.Request.Cookies[_cookieName.Value];
      }
    }

    internal Dictionary<string, string> CookieValues
    {
      get
      {
        var result = new Dictionary<string, string>();
        if ((HttpContext.Current != null) && (RequestCookie != null))
        {
          var nvc = RequestCookie.Values;
          foreach (var key in nvc.AllKeys)
          {
            result.Add(key, nvc[key]);
          }
        }
        return result;
      }
      set
      {
        if (HttpContext.Current != null)
        {
          HttpCookie splitValueCookie;
          if (HttpContext.Current.Response.Cookies.AllKeys.Contains(_cookieName.Value))
          {
            splitValueCookie = HttpContext.Current.Response.Cookies[_cookieName.Value];
          }
          else
          {
            splitValueCookie = _siteContext.Value.NewCrossDomainMemCookie(_cookieName.Value);
            if (splitValueCookie != null)
            {
              HttpContext.Current.Response.Cookies.Set(splitValueCookie);
            }
          }
          if (splitValueCookie != null)
          {
            foreach (var activeTest in value)
            {
              splitValueCookie.Values.Add(activeTest.Key, activeTest.Value);
            }
          }
        }
      }
    }

 }
}
