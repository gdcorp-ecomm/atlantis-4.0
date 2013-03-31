using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  public class CountrySubdomainLocalizationProvider : LocalizationProvider, ILocalizationProvider
  {
    private Lazy<ISiteContext> _siteContext;
    private Lazy<CountrySiteCookie> _countrySiteCookie;

    public CountrySubdomainLocalizationProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _countrySiteCookie = new Lazy<CountrySiteCookie>(() => { return new CountrySiteCookie(Container); });
    }

    protected override string DetermineCountrySite()
    {
      string result = _WWW;

      if ((HttpContext.Current != null) && (_siteContext.Value.ContextId == 1))
      {
        string subdomain = GetCountrySubdomain();
        if (IsValidCountrySubdomain(subdomain))
        {
          result = subdomain;
        }

        if ((!_countrySiteCookie.Value.HasValue) || (!_countrySiteCookie.Value.Value.Equals(result, StringComparison.OrdinalIgnoreCase)))
        {
          _countrySiteCookie.Value.Value = result;
        }
      }

      return result;
    }

    private string GetCountrySubdomain()
    {
      string result = string.Empty;
      try
      {
        string host = HttpContext.Current.Request.Url.Host;
        IProxyContext proxyContext;
        if (Container.TryResolve(out proxyContext))
        {
          host = proxyContext.ContextHost;
        }

        if ((host.Length > 2) && (host[2] == '.'))
        {
          result = host.Substring(0, 2);
        }
      }
      catch { }

      return result;
    }
  }
}
