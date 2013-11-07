using Atlantis.Framework.Interface;
using System;
using System.Web;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class CountryCookieLocalizationProvider : LocalizationProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;

    public CountryCookieLocalizationProvider(IProviderContainer container)
      :base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    protected override string DetermineCountrySite()
    {
      string result = _WWW;

      if ((HttpContext.Current != null) && (_siteContext.Value.ContextId == 1))
      {
        //  QueryString
        if (LocalizationProvider.TryGetRegionSiteFromQueryString(out result) && !string.IsNullOrEmpty(result) && IsValidCountrySubdomain(result))
        {
          SetCountrySiteCookie(result);
          return result;
        }
        
        //  Cookie
        if ((CountrySiteCookie.Value.HasValue) && (IsValidCountrySubdomain(CountrySiteCookie.Value.Value)))
        {
          result = CountrySiteCookie.Value.Value;
          SetCountrySiteCookie(result);
          return result;
        }

        //   IP Address
        result = GetCountrySiteFromIpCountry();
        if (!string.IsNullOrEmpty(result) && (IsValidCountrySubdomain(result)))
        {
          SetCountrySiteCookie(result);
          return result;
        }

        //  Default
        result = _WWW;
        SetCountrySiteCookie(result);
      }      

      return result;
    }

    private IGeoProvider _geoIpProvider;
    private IGeoProvider GeoIpProvider
    {
      get
      {
        if (_geoIpProvider == null)
        {
          Container.TryResolve(out _geoIpProvider);
        }

        return _geoIpProvider;
      }
    }

    private string GetCountrySiteFromIpCountry()
    {
      string result = null;

      if (GeoIpProvider != null)
      {
        result = GeoIpProvider.RequestCountryCode;

        if (!string.IsNullOrEmpty(result) && result.Equals("US", StringComparison.OrdinalIgnoreCase))
        {
          result = _WWW;
        }
      }

      return result;
    }
  }
}
