using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingCookieOverride
  {
    readonly IProviderContainer _container;
    readonly Lazy<string> _cookieName;
    readonly Lazy<ISiteContext> _siteContext;
    readonly Lazy<IShopperContext> _shopperContext;

    internal SplitTestingCookieOverride(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => _container.Resolve<IShopperContext>());
      _cookieName = new Lazy<string>(LoadCookieName);
    }

    private string LoadCookieName()
    {
      var shopperId = _shopperContext.Value.ShopperId.ToString(CultureInfo.InvariantCulture);
      if (string.IsNullOrEmpty(shopperId))
      {
        shopperId = "0";
      }

      return "SplitTestingOverride" + _siteContext.Value.PrivateLabelId.ToString(CultureInfo.InvariantCulture) + "_" + shopperId;
    }

    private HttpCookie RequestCookie
    {
      get
      {
        return HttpContext.Current.Request.Cookies[_cookieName.Value];
      }
    }

    public Dictionary<string, string> CookieValues
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

          HttpContext.Current.Response.Cookies.Set(splitValueCookie);
        }
      }
    }

 }
}
