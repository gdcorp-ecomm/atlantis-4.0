using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  internal class SplitTestingCookie
  {
    private const int MinExpirationHours = 1;
    private const int MaxExpirationHours = 168;
    private const int StdExpirationHours = 24;
    private const string CookielifeHours = "ATLANTIS_SPLITPROVIDER_COOKIELIFE_HOURS";

    readonly IProviderContainer _container;
    readonly Lazy<string> _cookieName;
    readonly Lazy<ISiteContext> _siteContext;

    internal SplitTestingCookie(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _cookieName = new Lazy<string>(LoadCookieName);
    }

    private string LoadCookieName()
    {
      return "SplitTesting" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
    }

    private HttpCookie RequestCookie
    {
      get
      {
        return HttpContext.Current.Request.Cookies[_cookieName.Value];
      }
    }

    internal Dictionary<string, string> Value
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
          HttpCookie splitValueCookie = _siteContext.Value.NewCrossDomainMemCookie(_cookieName.Value);

          foreach (var activeTest in value)
          {
            splitValueCookie.Values.Add(activeTest.Key, activeTest.Value);
          }
          splitValueCookie.Expires = CookieExpirationDate();

          HttpContext.Current.Response.Cookies.Set(splitValueCookie);
        }
      }
    }

    private DateTime CookieExpirationDate()
    {
      int expiration;
      string expirationHours = DataCache.DataCache.GetAppSetting(CookielifeHours);
      if (int.TryParse(expirationHours, out expiration))
      {
        expiration = expiration > MaxExpirationHours ? MaxExpirationHours : expiration;
        expiration = expiration < MinExpirationHours ? MinExpirationHours : expiration;
      }
      else
      {
        expiration = StdExpirationHours;
      }
      DateTime expirationDate = DateTime.Now.AddHours(expiration);
      return expirationDate;
    }
  }
}
