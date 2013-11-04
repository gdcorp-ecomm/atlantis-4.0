using Atlantis.Framework.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Localization
{
  public class CountrySubdomainLocalizationProvider : LocalizationProvider
  {
    private Lazy<ISiteContext> _siteContext;

    public CountrySubdomainLocalizationProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
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

        if ((!CountrySiteCookie.Value.HasValue) || (!CountrySiteCookie.Value.Value.Equals(result, StringComparison.OrdinalIgnoreCase)))
        {
          CountrySiteCookie.Value.Value = result;
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

        int dotIdx = host.IndexOf('.');
        if (dotIdx >= 2)
        {
          result = host.Substring(0, dotIdx);
        }
      }
      catch { }

      return result;
    }
  }
}
